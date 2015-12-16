using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.UnitTests
{
    [ControlExtensionProvider(typeof(ListView))]
    public static class TestListViewExtension
    {

        public static ListViewGrid GetGridValues(this ListView Target) 
        {

            var results = new ListViewGrid(Target);

            return results;

        }

        public static String GetGridColumnName(this ListView Target, Int32 Index)
        {

            return Target.Columns[Index].Text;

        }

        [Serializable] 
        public class ListViewGrid
        {

            protected Dictionary<String, List<String>> Grid;

            public ListViewGrid()
            {

            }

            public ListViewGrid(ListView listView)
            {

                Grid = new Dictionary<string, List<string>>();

                foreach (ColumnHeader header in listView.Columns) {

                    Grid.Add(header.Text, new List<String>());

                }


                List<String> columnIndex = new List<String>(Grid.Keys);

                foreach (ListViewItem item in listView.Items)
                {

                    var columnCount = 0;

                    foreach (var value in item.SubItems)
                    {

                        Grid[columnIndex[columnCount]].Add(value.ToString());

                        columnCount++;

                    }


                }


            }


        }


    }
}
