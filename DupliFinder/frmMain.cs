using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DupliFinder.Properties;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using DupliFinder.Processing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace DupliFinder
{
    public partial class frmMain : Form
    {
        private BackgroundWorker _bw;
        private object _comparisonsLock = new object();
        private List<string> _workingDirectories;
        private List<int> _processed;

        private System.Threading.Timer _timer;

        /// <summary>
        /// A list of images that were found during the search stage.
        /// </summary>
        private List<ComparableBitmap> _bitmapsFound;

        /// <summary>
        /// This is where results are put into when the algorithm is running.  Images are binned according to how similar they are, with
        /// some minimum threshold being the requirement for two images to be associated with one another.
        /// </summary>
        private Dictionary<ComparableBitmap, List<ComparableBitmap>> _binnedImages;

        /// <summary>
        /// Our in-use algorithm.  Populated by the drop-down menu on the GUI.
        /// </summary>
        private ComparisonAlgorithm _comparisonAlgInUse = null;

        private float _compareThreshold;
        /// <summary>
        /// ƒубликаты одного изображени€
        /// </summary>
        private KeyValuePair<ComparableBitmap, List<ComparableBitmap>> _selectedKeyValuePair;

        // Stuff for sorting the form
        private ListViewColumnSorter lvwColumnSorter;

        private const int _marginBetweenPB = 6;

        public frmMain()
        {
            InitializeComponent();
            btSearch.Text = Resources.strSearch;
            btRemOrig.Visible = btRemDup.Visible = false;

            lvDuplicates.ContextMenuStrip = new DuplicatesContextMenu(lvResults, lvDuplicates);
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            lvDuplicates.ListViewItemSorter = lvwColumnSorter;

        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                tbPath.Text = fd.SelectedPath;
                _workingDirectories = new List<string>();
                _workingDirectories.Add(tbPath.Text);
                btSearch.Enabled = true;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            _workingDirectories = getData(e.Data);
            if (_workingDirectories.Count > 0)
            {
                btSearch.Enabled = true;
                tbPath.Text = _workingDirectories[0];
            }
            else
            {
                btSearch.Enabled = false;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (getData(e.Data).Count > 0)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btSearch_Click(object sender, EventArgs e)
        {

            if (btSearch.Text == Resources.strSearch)
            {
                // Search is beginning.  Let's disable a bunch of stuff so user doesn't monkey with it.
                cbAlgorithms.Enabled = trackBarSimilarityThreshold.Enabled = false;
                btSearch.Text = Resources.setCancel;
                startSearch();

            }
            else
            {
                cbAlgorithms.Enabled = trackBarSimilarityThreshold.Enabled = true;
                btSearch.Text = Resources.strSearch;
                cancelSearch();
            }
        }

        List<string> getData(IDataObject obj)
        {
            List<string> resp = new List<string>();
            if (obj.GetDataPresent(DataFormats.FileDrop))
            {
                string[] str = obj.GetData(DataFormats.FileDrop) as string[];
                if (str != null && str.Length > 0)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (Directory.Exists(str[i]))
                        {
                            resp.Add(str[i]);
                        }
                    }
                }
            }
            return resp;
        }

        private void cancelSearch()
        {
            if (_bw != null && _bw.IsBusy)
                _bw.CancelAsync();
        }

        void startSearch()
        {
            // We're starting a new search, so toss everything we have.  zap all of our listviews, reset picture boxes, etc.

            _comparisonAlgInUse = cbAlgorithms.SelectedItem as ComparisonAlgorithm;
            _compareThreshold = ((float)trackBarSimilarityThreshold.Value) / 100.0F;

            lvResults.Items.Clear();
            lvDuplicates.Items.Clear();
            ClearPictureBoxes();

            btRemOrig.Visible = btRemDup.Visible = false;
            OnNewDuplicateFound += new EventHandler(frmMain_OnNewDuplicateFound);

            pbWorkingAll.Value = 0;
            _bw = new BackgroundWorker();
            _bw.WorkerReportsProgress = _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            _bw.RunWorkerAsync();
        }

        void frmMain_OnNewDuplicateFound(object sender, EventArgs e)
        {
            _timer.Change(25, Timeout.Infinite);
        }

        public void fillData(Object stateInfo)
        {
            this.BeginInvoke((SendOrPostCallback)delegate
            {
                lock (_comparisonsLock)
                {
                    ListView.SelectedIndexCollection rIdx = lvResults.SelectedIndices;
                    ListView.SelectedIndexCollection dIdx = lvDuplicates.SelectedIndices;
                    int iR = rIdx.Count > 0 ? rIdx[0] : -1; //что значит?  if (rIdx.Count > 0) iR = rIdx[0] else iR = -1
                    int iD = dIdx.Count > 0 ? dIdx[0] : -1;

                    this.MinimumSize = new Size(539, 442);
                    lvResults.Items.Clear();
                    lvDuplicates.Items.Clear();
                    foreach (ComparableBitmap cb in _binnedImages.Keys)
                    {
                        List<ComparableBitmap> val;
                        if (_binnedImages.TryGetValue(cb, out val) && val.Count > 0) //если заполнили val и > 0
                        {
                            string name = cb.Path.Substring(cb.Path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                            //lvResults.Items.Add(name);
                            ListViewItem item = new ListViewItem(new string[] {
                                name,
                                Path.GetDirectoryName(cb.Path)});
                            item.Tag = cb.Path;
                            lvResults.Items.Add(item);
                        }
                    }
                    if (lvResults.Items.Count <= iR)
                        iR = lvResults.Items.Count - 1;

                    if (lvDuplicates.Items.Count <= iD)
                        iD = lvDuplicates.Items.Count - 1;

                    if (iR != -1 & lvResults.Items.Count > 0)
                    {
                        lvResults.SelectedIndices.Add(iR);
                    }
                    if (iD != -1 & lvDuplicates.Items.Count > 0)
                    {
                        lvDuplicates.SelectedIndices.Add(iD);
                    }
                }

            }, new object[] { null });
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_binnedImages == null || _binnedImages.Count < 1)
                MessageBox.Show(this, Resources.msgNoDups, "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // reset search string name.
            btSearch.Text = Resources.strSearch;

            // re-enable all of our gui elements
            cbAlgorithms.Enabled = trackBarSimilarityThreshold.Enabled = true;

            OnNewDuplicateFound -= new EventHandler(frmMain_OnNewDuplicateFound);
            _bitmapsFound.Clear();

            for (int u = 0; u < lvResults.Columns.Count; u++)
                lvResults.Columns[u].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbWorkingAll.Value = e.ProgressPercentage;
            if (e.UserState.GetType() == typeof(string))
            {
                lblWorking.Text = (string)e.UserState;
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            _bitmapsFound = new List<ComparableBitmap>();
            for (int i = 0; i < _workingDirectories.Count; i++)
            {
                if (_bw.CancellationPending)
                    break;
                enumFiles(_workingDirectories[i], (int)((double)(i / 2) / (double)_workingDirectories.Count * 100));
                compareFiles((int)((double)(i) / (double)_workingDirectories.Count * 100));
            }
        }

        /// <summary>
        /// Fetches all of the images
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extentions"></param>
        /// <param name="includeSubdirs"></param>
        /// <returns></returns>
        string[] getImgFiles(string path, string[] extentions, bool includeSubdirs)
        {
            List<string> files = new List<string>();
            foreach (string extension in extentions)
            {
                files.AddRange(Directory.GetFiles(path, string.Format("{0}", extension), includeSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            }

            // remove any duplicates (at some point, someone made a rather moronic change to Directory.GetFiles() where it behaves 
            // differently for patters of *.xxx, where "xxx" is exactly three characters long.  See
            // http://msdn.microsoft.com/en-us/library/wz42302f.aspx for more info.
            for (int x = 0; x < files.Count; x++)
            {
                string file = files[x];
                for (int y = x + 1; y < files.Count; y++)
                {
                    if (file == files[y])
                        files.RemoveAt(y);
                }
            }

            return files.ToArray();
        }

        /// <summary>
        /// helper function to enumerate supported image extensions.
        /// </summary>
        /// <returns></returns>
        string[] supportedFileExtensions()
        {
            List<string> extensions = new List<string>();
            foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageDecoders())
            {
                // some image formats include multiple extensions, delimited w/ a semicolon.
                // We'll split this out and include them individually.
                extensions.AddRange(ici.FilenameExtension.Split(';'));
            }
            return extensions.ToArray();
        }

        void enumFiles(string path, int progress)
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            string[] files = getImgFiles(path, supportedFileExtensions(), cbIncSubfolders.Checked);
            for (int i = 0; i < files.Length; i++)
            {
                if (_bw.CancellationPending)
                    break;

                _bitmapsFound.Add(new ComparableBitmap(files[i], _comparisonAlgInUse.CompareImageWidth, _comparisonAlgInUse.CompareImageHeight));
                _bw.ReportProgress((int)((double)i / (double)files.Length * 100), Resources.strLoading);
            }
#if DEBUG
            sw.Stop();
            Console.WriteLine(String.Format("Loading Time: {0} msec", sw.ElapsedMilliseconds));
#endif
        }

        void compareFiles(int progress)
        {
            if (_bitmapsFound == null)
                return;

            _binnedImages = new Dictionary<ComparableBitmap, List<ComparableBitmap>>();
            _processed = new List<int>();

            // Measure our elapsed time.
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // First, let's measure how comparable each bitmap is to every other bitmap.
            for(int source = 0; source < _bitmapsFound.Count; source++)
            {
                if (_bw.CancellationPending)
                    break;

                _bw.ReportProgress((int)((((double)source) / ((double)_bitmapsFound.Count)) * 100), string.Format(Resources.strProceessing,
                    _bitmapsFound[source].Path.Substring(_bitmapsFound[source].Path.LastIndexOf(Path.DirectorySeparatorChar) + 1)));

                ThreadQueue q = new ThreadQueue(Environment.ProcessorCount);

                // We compare this index to every index _after_ it (this is because comparing A to B is the same as B to A
                // so instead of n^2 comparisons, it's n(n-1) / 2 comparisons.  Same big-O, but still cheaper.
                for (int destination = source + 1; destination < _bitmapsFound.Count; destination++)
                {
                    q.QueueUserWorkItem((WaitCallback)delegate(object a)
                    {
                        int dest = (int)a;
                        float k = _comparisonAlgInUse.Similarity(_bitmapsFound[source].GrayscaleData, _bitmapsFound[dest].GrayscaleData);
                        if (k >= _compareThreshold)
                        {
                            // So, this match is comparable.  Let's add it as a match to both images
                            lock (_comparisonsLock)
                            {
                                _bitmapsFound[source].ClosestMatches.Add(_bitmapsFound[dest], k);
                                _bitmapsFound[dest].ClosestMatches.Add(_bitmapsFound[source], k);

                                if( !_binnedImages.ContainsKey(_bitmapsFound[source]))
                                    _binnedImages.Add(_bitmapsFound[source], new List<ComparableBitmap>());

                                if (!_binnedImages.ContainsKey(_bitmapsFound[dest]))
                                    _binnedImages.Add(_bitmapsFound[dest], new List<ComparableBitmap>());

                                _binnedImages[_bitmapsFound[source]].Add(_bitmapsFound[dest]);
                                _binnedImages[_bitmapsFound[dest]].Add(_bitmapsFound[source]);
                                OnNewDuplicateFound(this, null);
                            }
                        }
                    },destination );
                }

                q.WaitAll();
            }

            sw.Stop();

            _bw.ReportProgress(100, String.Format("Processing Time: {0} msec", sw.ElapsedMilliseconds));
        }

        event EventHandler OnNewDuplicateFound;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //pbOrig.Width = pbOrig.Height = pbDup.Width = pbDup.Height = splitContainer1.Panel2.Width / 2;
            //lvDuplicates.Height = splitContainer1.Panel2.Height - pbDup.Height - 24;
            pbOrig.Width = pbOrig.Height = pbDup.Width = pbDup.Height = splitContainer1.Panel1.Height / 2 - _marginBetweenPB / 2;
            splitContainer1.SplitterDistance = pbOrig.Width + splitContainer1.SplitterWidth;

            RefreshPictureBoxes();
        }

        /// <summary>
        /// Quick helper method to "refresh" the picture boxes.
        /// </summary>
        private void RefreshPictureBoxes()
        {
            if (pbDup.Image != null && pbDup.Tag != null)
            {
                pbDup.Image.Dispose();
                pbDup.Image = null;

                // Let's resize this image, since user changed GUI size
                Image b = Image.FromFile(pbDup.Tag.ToString());
                pbDup.Image = ComparableBitmap.resizeImage(b, (int)(b.Width * getRatio(b.Size)), (int)(b.Height * getRatio(b.Size)));

                b.Dispose();
                b = null;
            }

            if (pbOrig.Image != null && pbOrig.Tag != null)
            {
                pbOrig.Image.Dispose();
                pbOrig.Image = null;

                // Let's resize this image, since user changed GUI size
                Image b = Image.FromFile(pbOrig.Tag.ToString());
                pbOrig.Image = ComparableBitmap.resizeImage(b, (int)(b.Width * getRatio(b.Size)), (int)(b.Height * getRatio(b.Size)));

                b.Dispose();
                b = null;
            }
        }

        KeyValuePair<ComparableBitmap, List<ComparableBitmap>> getItemByParticalKeyName(string name)
        {
            foreach (ComparableBitmap b in _binnedImages.Keys)
            {
                if (b.Path.Contains(name))
                    return new KeyValuePair<ComparableBitmap, List<ComparableBitmap>>(b, _binnedImages[b]);
            }
            return new KeyValuePair<ComparableBitmap, List<ComparableBitmap>>();

        }

        double getRatio(Size s)
        {
            return (s.Width < s.Height) ? (double)pbOrig.Width / (double)s.Width : (double)pbOrig.Height / (double)s.Height;
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvDuplicates.Items.Clear();
            if (lvResults.SelectedItems.Count > 0)
            {
                string str = lvResults.SelectedItems[0].Text;
                _selectedKeyValuePair = getItemByParticalKeyName(str);

                displayPreviewImages();
                foreach (ComparableBitmap cb in _selectedKeyValuePair.Value)
                {
                    // TODO we should add on any additional columns specified by the comparison algorithm, 
                    // so the algorithm itself can have some custom output besides name, size and similarity
                    ListViewItem item = new ListViewItem(new string[] {
                        cb.Path.Substring(cb.Path.LastIndexOf(Path.DirectorySeparatorChar) + 1),
                        string.Format("{0} x {1}", cb.OrgImageSize.Width, cb.OrgImageSize.Height), 
                        string.Format("{0:N2}",_selectedKeyValuePair.Key.ClosestMatches[cb]),
                        Path.GetDirectoryName(cb.Path)});
                    item.Tag = cb.Path;
                    lvDuplicates.Items.Add(item);
                }

                lvDuplicates.Columns[3].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

                // Let's do a default sort based on our similarity column
                lvwColumnSorter.SortColumn = -1;    // so we go to a "default"
                Sort(2);                            // column 2 is our "similarity" column, which is probably most pertinent
            }
        }

        private void lvDuplicates_SelectedIndexChanged(object sender, EventArgs e)
        {
            // we only support selecting a single item at a time, of course...
            if (_selectedKeyValuePair.Key != null && lvDuplicates.SelectedIndices.Count == 1)
            {
                displayPreviewDuplicate(lvDuplicates.SelectedItems[0].Tag as string);
            }
        }

        private void displayPreviewDuplicate(string path)
        {
            if (_selectedKeyValuePair.Key != null)
            {
                Image b = Image.FromFile(path);
                if (pbDup.Image != null)
                {
                    pbDup.Image.Dispose();
                    pbDup.Image = null;
                }

                pbDup.Image = ComparableBitmap.resizeImage( b, (int)(b.Width * getRatio(b.Size)), (int)(b.Height * getRatio(b.Size)));
                pbDup.Tag = path;
                btRemDup.Visible = true;

                // make sure we hold no outstanding references to the source material.
                b.Dispose();
                b = null;
            }
        }

        private void displayPreviewImages()
        {
            if (_selectedKeyValuePair.Key != null)
            {
                Image b = Image.FromFile(_selectedKeyValuePair.Key.Path);
                if (pbOrig.Image != null)
                {
                    pbOrig.Image.Dispose();
                    pbOrig.Image = null;
                }
                pbOrig.Image = ComparableBitmap.resizeImage(b,(int)(b.Width * getRatio(b.Size)), (int)(b.Height * getRatio(b.Size)));
                pbOrig.Tag = _selectedKeyValuePair.Key.Path;
                if (pbDup.Image != null)
                {
                    pbDup.Image.Dispose();
                    pbDup.Image = null;
                    btRemDup.Visible = false;
                }
                btRemOrig.Visible = true;

                // make sure we hold no outstanding references to the source material.
                b.Dispose();
                b = null;
            }
        }

        private void ClearPictureBoxes()
        {
            if (pbOrig.Image != null)
            {
                pbOrig.Image.Dispose();
                pbOrig.Image = null;
            }
            if (pbDup.Image != null)
            {
                pbDup.Image.Dispose();
                pbDup.Image = null;
            }
        }

        /// <summary>
        /// Deletes the file from disk. If it's in use, prompts the user.
        /// </summary>
        /// <param name="pb"></param>
        /// <returns></returns>
        private bool DeleteFile(string filePath)
        {
            while (File.Exists(filePath))
            {
                try
                {
                    //File.Delete(filePath);
                    RecycleBin.DeleteFileOrFolder(filePath);
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    Debug.WriteLine(String.Format("Exception in {0}: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));

                    DialogResult dr = MessageBox.Show(this, string.Format(Resources.strFileReadOnly, filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar) + 1)),
                        Resources.strFileReadOnlyHeader,
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Cancel)
                        break;
                }
                catch (Exception ex)
                {
                    // Probably IOException...
                    Debug.WriteLine(String.Format("Exception in {0}: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));

                    DialogResult dr = MessageBox.Show(this, string.Format(Resources.strFileInUse, filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar) + 1)), Resources.strFileInUseHeader,
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Cancel)
                        break;
                }
            }

            // We return the true state of the file; whether or not it's been toasted.
            return !File.Exists(filePath);
        }


        /// <summary>
        /// Called when the original delete button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRemOrig_Click(object sender, EventArgs e)
        {
            if (pbOrig.Tag is string &&
                MessageBox.Show(this,
                string.Format(Resources.msgSure, pbOrig.Tag.ToString().Substring(pbOrig.Tag.ToString().LastIndexOf(Path.DirectorySeparatorChar) + 1)), "Delete image",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Let's delete the image _first_, and then remove it from our listing.  That way, if there's an 
                // error, user doesn't have to perform a second scan.
                if (DeleteFile(pbOrig.Tag.ToString()) == true)
                {
                    ClearPictureBoxes();

                    lock (_comparisonsLock)
                    {
                        _binnedImages.Remove(_selectedKeyValuePair.Key);

                        // Let's also make sure we remove this image from any other binned images
                        foreach( List<ComparableBitmap> listOfImages in _binnedImages.Values )
                        {
                            for (int x = 0; x < listOfImages.Count; x++)
                            {
                                if (listOfImages[x].Path == _selectedKeyValuePair.Key.Path)
                                {
                                    listOfImages.RemoveAt(x);
                                    break;
                                }
                            }
                        }
                    }

                    _selectedKeyValuePair.Key.Dispose();

                    btRemOrig.Visible = false;
                    btRemDup.Visible = false;
                    _timer.Change(0, Timeout.Infinite);
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Called when the duplicate delete button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRemDup_Click(object sender, EventArgs e)
        {
            if (pbDup.Tag is string &&
                MessageBox.Show(this,
                string.Format(Resources.msgSure, pbDup.Tag.ToString().Substring(pbDup.Tag.ToString().LastIndexOf(Path.DirectorySeparatorChar) + 1)), "Delete image",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DeleteFile(pbDup.Tag.ToString()) == true)
                {
                    if (pbDup.Image != null)
                    {
                        pbDup.Image.Dispose();
                        pbDup.Image = null;
                    }

                    ComparableBitmap cb = _selectedKeyValuePair.Value[lvDuplicates.SelectedIndices[0]];
                    if (_binnedImages.ContainsKey(cb))
                        _binnedImages.Remove(cb);

                    // Let's also make sure we remove this image from any other binned images
                    foreach (List<ComparableBitmap> listOfImages in _binnedImages.Values)
                    {
                        for (int x = 0; x < listOfImages.Count; x++)
                        {
                            if (listOfImages[x].Path == pbDup.Tag.ToString())
                            {
                                listOfImages.RemoveAt(x);
                                break;
                            }
                        }
                    }

                    btRemDup.Visible = false;

                    _timer.Change(0, Timeout.Infinite);
                    GC.Collect();
                }
            }
        }

        private void pbOrig_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null && pb.Tag is string)
            {
                Process.Start(pb.Tag.ToString());
            }
        }

        private void pbOrig_Resize(object sender, EventArgs e)
        {
            /*pbDup.Location = new Point(pbOrig.Location.X + pbOrig.Width + 6, pbDup.Location.Y);
            lvDuplicates.Location = new Point(lvDuplicates.Location.X, pbDup.Location.Y + pbDup.Height + 3);
            btRemOrig.Location = new Point(pbOrig.Location.X + pbOrig.Width - btRemOrig.Width, pbOrig.Location.Y + pbOrig.Height - btRemOrig.Height);
            btRemDup.Location = new Point(pbDup.Location.X + pbDup.Width - btRemDup.Width, pbDup.Location.Y + pbDup.Height - btRemDup.Height);*/

            pbDup.Location = new Point(pbDup.Location.X, pbOrig.Location.Y + pbOrig.Height + _marginBetweenPB);
            // –асположение кнопок удалени€
            btRemOrig.Location = new Point(pbOrig.Location.X + pbOrig.Width - btRemOrig.Width, pbOrig.Location.Y + pbOrig.Height - btRemOrig.Height);
            btRemDup.Location = new Point(pbDup.Location.X + pbDup.Width - btRemDup.Width, pbDup.Location.Y + pbDup.Height - btRemDup.Height);
        }


        private static string GetAssemblyName()
        {
            Process pr = Process.GetCurrentProcess();
            return pr.ProcessName + ".exe";
        }



        private void frmMain_Load(object sender, EventArgs e)
        {
            // Let's populate our combo box with any algorithms we can find.
            Assembly assem = Assembly.LoadFrom("DupliFinder.exe");

            Type[] types = assem.GetTypes();

            // Zap the default item.
            cbAlgorithms.Items.Clear();

            // Let's locate all implementations of comparison algorithm, and we'll add them to the drop-down box.
            foreach (Type t in types)
            {
                if( t.BaseType.FullName == "DupliFinder.Processing.ComparisonAlgorithm" )
                {
                    ComparisonAlgorithm ca = (ComparisonAlgorithm) Activator.CreateInstance( t );
                    cbAlgorithms.Items.Add(ca);
                }
            }

            // Let's setup our timer.
            TimerCallback timercallback = new TimerCallback(fillData);
            _timer = new System.Threading.Timer(timercallback);

            // Lastly, let's set a default algorithm
            cbAlgorithms.SelectedItem = cbAlgorithms.Items[1];
            trackBarSimilarityThreshold.Value = Convert.ToInt32((cbAlgorithms.SelectedItem as ComparisonAlgorithm).SimilarityThreshold) * 100;
        }

        private void cbAlgorithms_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // update our slider bar with the default value of the selected item.
            if (cbAlgorithms.SelectedItem is ComparisonAlgorithm)
            {
                ComparisonAlgorithm ca = cbAlgorithms.SelectedItem as ComparisonAlgorithm;
                trackBarSimilarityThreshold.Value = Convert.ToInt32(ca.SimilarityThreshold) * 100;
            }
        }

        private void trackBarSimilarityThreshold_ValueChanged(object sender, EventArgs e)
        {
            // Set the value of the trackbar in our text box
            SetTextThreshBasedOnTrackBar();
        }

        private void SetTextThreshBasedOnTrackBar()
        {
            float trackVal = (float)trackBarSimilarityThreshold.Value;
            labelSimilarityThreshold.Text = (trackVal / 100.0).ToString();
        }

        private void lvDuplicates_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Sort(e.Column);
        }

        private void Sort(int column)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to descending.
                lvwColumnSorter.SortColumn = column;
                lvwColumnSorter.Order = SortOrder.Descending;
            }

            // Perform the sort with these new sort options.
            this.lvDuplicates.Sort();
        }

    }
}