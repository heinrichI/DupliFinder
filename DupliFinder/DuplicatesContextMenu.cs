using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO; //CancelEventHandler

namespace DupliFinder
{
    class DuplicatesContextMenu : ContextMenuStrip
    {
        private ListView _lvDuplicates;
        private ListView _lvResults;

        private ToolStripMenuItem m_renameImageLikeNeighbourItem;
        private ToolStripMenuItem m_moveImageToNeighbourItem;
        private ToolStripMenuItem m_moveImageAndRenameToNeighbourItem;

        public DuplicatesContextMenu(ListView lvResults, ListView lvDuplicates)
        {
            _lvResults = lvResults;
            _lvDuplicates = lvDuplicates;
            InitializeComponents();
            UpdateStrings();
            //Resources.Strings.OnCurrentChange += new Resources.Strings.CurrentChangeHandler(UpdateStrings);
            Opening += new CancelEventHandler(OnOpening);
        }

        private void InitializeComponents()
        {
            RenderMode = ToolStripRenderMode.System;

            m_renameImageLikeNeighbourItem = InitFactory.MenuItem.Create(null, null, new EventHandler(this.RenameLikeNeighbour));
            m_moveImageToNeighbourItem = InitFactory.MenuItem.Create(null, null, MoveImageToNeighbour);
            m_moveImageAndRenameToNeighbourItem = InitFactory.MenuItem.Create(null, null, new EventHandler(this.MoveAndRenameToNeighbour));

            Items.Add(new ToolStripSeparator());
        }

        private void OnOpening(object sender, EventArgs e)
        {
            Items.Clear();

            Items.Add(m_renameImageLikeNeighbourItem);
            if (MoveImageToNeighbourEnable())
            {
                Items.Add(m_moveImageToNeighbourItem);
            }
            if (MoveAndRenameToNeighbourEnable())
            {
                Items.Add(m_moveImageAndRenameToNeighbourItem);
            }
        }

        private void UpdateStrings()
        {
            //Strings s = Resources.Strings.Current;

            m_renameImageLikeNeighbourItem.Text = "Переименовать как главную";
            m_moveImageToNeighbourItem.Text = "Переместить картинку к главной";
            m_moveImageAndRenameToNeighbourItem.Text = "Переместить и переименовать к главной";
        }

        /// <summary>
        /// Перенести к соседней
        /// </summary>
        private void MoveImageToNeighbour(object sender, EventArgs e)
        {
            ListViewItem itemSource = _lvDuplicates.FocusedItem;
            ListViewItem itemMain = _lvResults.FocusedItem;

            if (itemSource != null && itemMain != null)
            {
                string sourcePath = itemSource.Tag as string;
                string mainPath = itemMain.Tag as string;
                string sourceDir = Path.GetDirectoryName(sourcePath);
                string mainDir = Path.GetDirectoryName(mainPath);

                string targetPath = Path.Combine(mainDir, Path.GetFileName(sourcePath));

                try
                {
                    if (!System.IO.File.Exists(targetPath))
                    {
                        new FileInfo(sourcePath).MoveTo(targetPath);
                        UpdateLV(itemSource, mainDir, targetPath);
                    }
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        /// <summary>
        /// Проверка на существование файла в папке соседней картинки с именем текущей картинки.
        /// </summary>
        /// <returns></returns>
        private bool MoveImageToNeighbourEnable()
        {
            ListViewItem itemSource = _lvDuplicates.FocusedItem;
            ListViewItem itemMain = _lvResults.FocusedItem;

            if (itemSource != null && itemMain != null)
            {
                string sourcePath = itemSource.Tag as string;
                string mainPath = itemMain.Tag as string;
                string sourceDir = Path.GetDirectoryName(sourcePath);
                string mainDir = Path.GetDirectoryName(mainPath);

                if (mainDir != sourceDir)
                {
                    StringBuilder targetPath = new StringBuilder(mainDir);
                    targetPath.Append(Path.DirectorySeparatorChar);
                    targetPath.Append(Path.GetFileName(sourcePath));
                    FileInfo fileInfo = new FileInfo(targetPath.ToString());
                    return !fileInfo.Exists;
                }
            }

            return false;
        }


        private void RenameLikeNeighbour(object sender, EventArgs e)
        {
            renameFileLikeNeighbour();
        }

        private void renameFileLikeNeighbour()
        {
            ListViewItem itemSource = _lvDuplicates.FocusedItem;
            ListViewItem itemMain = _lvResults.FocusedItem;

            if (itemSource != null && itemMain != null)
            {
                string sourcePath = itemSource.Tag as string;
                string mainPath = itemMain.Tag as string;
                string sourceDir = Path.GetDirectoryName(sourcePath);
                string mainDir = Path.GetDirectoryName(mainPath);

                string targetPath = Path.Combine(sourceDir, Path.GetFileName(mainPath));

                try
                {
                    targetPath = Rename.SimilarRename(targetPath, sourcePath);
                    new FileInfo(sourcePath).MoveTo(targetPath);
                    UpdateLV(itemSource, sourceDir, targetPath);
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Проверка на то, что директории у картинок разные.
        /// </summary>
        private bool MoveAndRenameToNeighbourEnable()
        {
            ListViewItem itemSource = _lvDuplicates.FocusedItem;
            ListViewItem itemMain = _lvResults.FocusedItem;

            if (itemSource != null && itemMain != null)
            {
                string sourcePath = itemSource.Tag as string;
                string mainPath = itemMain.Tag as string;
                string sourceDir = Path.GetDirectoryName(sourcePath);
                string mainDir = Path.GetDirectoryName(mainPath);

                if (mainDir != sourceDir)
                    return true;
            }

            return false;
        }


        private void MoveAndRenameToNeighbour(object sender, EventArgs e)
        {
            ListViewItem itemSource = _lvDuplicates.FocusedItem;
            ListViewItem itemMain = _lvResults.FocusedItem;

            if (itemSource != null && itemMain != null)
            {
                string sourcePath = itemSource.Tag as string;
                string mainPath = itemMain.Tag as string;
                string sourceDir = Path.GetDirectoryName(sourcePath);
                string mainDir = Path.GetDirectoryName(mainPath);

                try
                {
                    string targetPath = Rename.SimilarRename(mainPath, Path.GetFileName(sourcePath));
                    new FileInfo(sourcePath).MoveTo(targetPath);
                    UpdateLV(itemSource, mainDir, targetPath);
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateLV(ListViewItem item, string sourceDir, string targetPath)
        {
            item.SubItems[0].Text = Path.GetFileName(targetPath);
            item.SubItems[3].Text = sourceDir;
            item.Tag = targetPath;

            UpdateMainLV();
        }

        private void UpdateMainLV()
        {
           // _lvResults
        }
    }
}
