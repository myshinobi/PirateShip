using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PirateShip
{
    public partial class Debugger : Form
    {
        public PirateShip pirateShip;

        public Debugger(PirateShip pirateShip)
        {
            InitializeComponent();

            this.pirateShip = pirateShip;

            LoadPage();
        }

        public void LoadPage()
        {
            textBox1.Text = pirateShip.url.ToString();
            textBox2.Text = pirateShip.method.ToString();
            textBox3.Text = PirateShip.DefaultUserAgent;
            splitContainer1.Panel2Collapsed = true;
            splitContainer2.Panel2Collapsed = true;
            splitContainer3.Panel2Collapsed = true;
            splitContainer4.Panel2Collapsed = true;
            splitContainer5.Panel2Collapsed = true;
            listView1.Items.Clear();

            listView2.Items.Clear();
            listView3.Items.Clear();

            CookieCollection cookieCollection = pirateShip.GetCookies();
            int cookieCount = 0;
            foreach (Cookie cookie in cookieCollection)
            {
                cookieCount++;

                ListViewItem item = new ListViewItem(cookieCount.ToString());

                ListViewItem.ListViewSubItem nameItem = new ListViewItem.ListViewSubItem(item, cookie.Name);
                nameItem.Tag = cookie;
                item.SubItems.Add(nameItem);


                ListViewItem.ListViewSubItem valueItem = new ListViewItem.ListViewSubItem(item, cookie.Value);
                valueItem.Tag = cookie;
                item.SubItems.Add(valueItem);

                listView2.Items.Add(item);
            }

            //if (pirateShip.cookies != null)
            //{
            //    string[] cookieEnum = pirateShip.cookies.ToArray();
            //    for (int i = 0; i < pirateShip.cookies.Count(); i++)
            //    {
            //        string cookie = cookieEnum[i];

            //        ListViewItem item = new ListViewItem(i.ToString());

            //        ListViewItem.ListViewSubItem valueItem = new ListViewItem.ListViewSubItem(item, cookie);
            //        valueItem.Tag = cookie;
            //        item.SubItems.Add(valueItem);

            //        listView2.Items.Add(item);
            //    }
            //}

            if (!string.IsNullOrEmpty(pirateShip.responseBody))
            {
                tabControl1.SelectedTab = tabPage2;
                HttpStatusCode? httpStatusCode = pirateShip.statusCode;
                label5.Text = "Status: " + ((int?)httpStatusCode)?.ToString() + " " + httpStatusCode?.ToString();

                button1.Text = "Close";
            }
            else
            {
                tabControl3.Enabled = false;
            }

            //if (pirateShip.response != null)
            //{
            //    tabControl1.SelectedTab = tabPage2;
            //    System.Net.HttpStatusCode httpStatusCode = pirateShip.response.StatusCode;

            //    label5.Text = "Status: "+((int)httpStatusCode).ToString() + " " + httpStatusCode.ToString();
            //    richTextBox5.Text = pirateShip.response.ToString();
            //    richTextBox8.Text = pirateShip.response.RequestMessage.ToString();
            //    foreach (KeyValuePair<string, IEnumerable<string>> header in pirateShip.response.Headers)
            //    {

            //        ListViewItem item = new ListViewItem(header.Key);

            //        ListViewItem.ListViewSubItem valueItem = new ListViewItem.ListViewSubItem(item, Newtonsoft.Json.JsonConvert.SerializeObject(header.Value));
            //        valueItem.Tag = header;
            //        item.SubItems.Add(valueItem);

            //        listView3.Items.Add(item);

            //    }

            //    button1.Text = "Close";
            //}
            //else
            //{
            //    tabControl3.Enabled = false;
            //}

            //richTextBox7.Text = pirateShip.responseBody;


            //if (pirateShip.message != null)
            //{
            //    richTextBox4.Text = pirateShip.message.ToString();
            //    foreach (KeyValuePair<string,IEnumerable<string>> header in pirateShip.message.Headers)
            //    {

            //        ListViewItem item = new ListViewItem(header.Key);

            //        ListViewItem.ListViewSubItem valueItem = new ListViewItem.ListViewSubItem(item, Newtonsoft.Json.JsonConvert.SerializeObject(header.Value));
            //        valueItem.Tag = header;
            //        item.SubItems.Add(valueItem);

            //        listView1.Items.Add(item);


            //    }

            //    if (pirateShip.message.Content != null)
            //    {
            //        if (string.IsNullOrEmpty(pirateShip.postContent))
            //        {
            //            try
            //            {

            //                Task<string> task = pirateShip.message.Content.ReadAsStringAsync();
            //                task.Wait();
            //                pirateShip.postContent = task.Result;
            //            }
            //            catch (Exception)
            //            {


            //            }
            //        }

            //        richTextBox1.Text = pirateShip.postContent;
            //    }
            //}
        }
        

        private void Debugger_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                KeyValuePair<string, IEnumerable<string>> header = (KeyValuePair<string, IEnumerable<string>>)listView1.SelectedItems[0].SubItems[1].Tag;
                splitContainer2.Panel2Collapsed = true;
                listBox1.Items.Clear();
                foreach (string item in header.Value)
                {
                    listBox1.Items.Add(item);
                }

                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {

                string item = listBox1.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(item))
                {
                    richTextBox2.Text = item;
                    splitContainer2.Panel2Collapsed = false;
                }
                else
                {
                    splitContainer2.Panel2Collapsed = true;

                }
            }
            else
            {
                splitContainer2.Panel2Collapsed = true;

            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                Cookie cookie = (Cookie)listView2.SelectedItems[0].SubItems[1].Tag;

                richTextBox3.Text = cookie.Name+"="+cookie.Value;

                splitContainer3.Panel2Collapsed = false;
            }
            else
            {
                splitContainer3.Panel2Collapsed = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                KeyValuePair<string, IEnumerable<string>> header = (KeyValuePair<string, IEnumerable<string>>)listView3.SelectedItems[0].SubItems[1].Tag;
                splitContainer5.Panel2Collapsed = true;
                listBox2.Items.Clear();
                foreach (string item in header.Value)
                {
                    listBox2.Items.Add(item);
                }

                splitContainer4.Panel2Collapsed = false;
            }
            else
            {
                splitContainer4.Panel2Collapsed = true;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {

                string item = listBox2.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(item))
                {
                    richTextBox6.Text = item;
                    splitContainer5.Panel2Collapsed = false;
                }
                else
                {
                    splitContainer5.Panel2Collapsed = true;

                }
            }
            else
            {
                splitContainer5.Panel2Collapsed = true;

            }
        }
    }
}
