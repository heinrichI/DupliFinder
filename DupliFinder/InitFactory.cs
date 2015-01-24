using System;
using System.Collections.Generic;
using System.Text;

namespace DupliFinder
{
    /// <summary>
    /// Фабрика создает элементы GUI
    /// </summary>
    public class InitFactory
    {
        public static class MenuItem
        {
            public static System.Windows.Forms.ToolStripMenuItem Create(string image, object tag, EventHandler handler)
            {
                System.Windows.Forms.ToolStripMenuItem menuItem = new System.Windows.Forms.ToolStripMenuItem();
                /*if (image != null)
                {
                    menuItem.Image = Resources.Images.Get(image);
                    menuItem.ImageScaling = ToolStripItemImageScaling.None;
                }*/
                menuItem.Tag = tag;
                menuItem.Click += new EventHandler(handler);
                return menuItem;
            }

            public static System.Windows.Forms.ToolStripMenuItem Create(string image, object tag, EventHandler handler, bool checkedValue)
            {
                System.Windows.Forms.ToolStripMenuItem menuItem = Create(image, tag, handler);
                menuItem.CheckOnClick = true;
                menuItem.Checked = checkedValue;
                return menuItem;
            }
        };
    }
}
