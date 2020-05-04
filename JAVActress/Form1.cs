using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace JAVActress
{
    public partial class frm : Form
    {
        HttpClient client = new HttpClient();

        GlassyPanel panel = new GlassyPanel();

        public frm()
        {
            InitializeComponent();


            client.BaseAddress = new Uri("https://jav-rest-api-htpvmrzjet.now.sh/api/");
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

            textBox1.Text = "Mikami";
        }

        const string linkActress = "actress?name={0}";

        const string linkMovies = "videos/{0}";

        const string google = "https://www.google.com/search?q={0}";

        private async void button1_Click(object sender, EventArgs e)
        {
            //Mikami
            try
            {
                panel.Show();
                button1.Enabled = false;
                Application.DoEvents();

                if (textBox1.Text.Trim() != "")
                {
                    string getLinkActress = string.Format(linkActress, textBox1.Text.Trim());

                    var response = await client.GetStringAsync(getLinkActress);

                    var data = JsonConvert.DeserializeObject<ActressResult>(response);

                    if (data.Count > 0)
                    {
                        dataGridView1.Rows.Clear();

                        List<Actress> list = data.Result;

                        for (int num = 0; num < list.Count; num++)
                        {
                            Actress act = list[num];

                            this.dataGridView1.Rows.Add(num + 1, act.Id, act.Name, act.JapanName, "   Search   ");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Actress not found!");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button1.Enabled = true;
                panel.Hide();
                Application.DoEvents();
            }
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                panel.Show();
                Application.DoEvents();

                if (e.ColumnIndex == dataGridView1.Columns["Search"].Index && e.RowIndex >= 0)
                {
                    string code = dataGridView1[dataGridView1.Columns["Id"].Index, e.RowIndex].Value?.ToString();

                    string getlinkMovies = string.Format(linkMovies, code);

                    var response = await client.GetStringAsync(getlinkMovies);

                    var data = JsonConvert.DeserializeObject<MoviesResult>(response);

                    if (data.Count > 0)
                    {
                        dataGridView2.Rows.Clear();

                        List<Movies> list = data.Result;

                        for (int num = 0; num < list.Count; num++)
                        {
                            Movies act = list[num];

                            this.dataGridView2.Rows.Add(num + 1, "   Google   ", act.Code, act.Date, act.Name);
                        }

                    }

                    else
                    {
                        MessageBox.Show("Movies not found!");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                panel.Hide();
                Application.DoEvents();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                panel.Show();
                Application.DoEvents();
                if (e.ColumnIndex == dataGridView2.Columns["SearchGet"].Index && e.RowIndex >= 0)
                {
                    string code = dataGridView1[dataGridView2.Columns["SearchCode"].Index, e.RowIndex].Value?.ToString();

                    System.Diagnostics.Process.Start(string.Format(google, code));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                panel.Hide();
                Application.DoEvents();
            }
        }

        private void frm_Load(object sender, EventArgs e)
        {   
            this.Controls.Add(panel);
            panel.BringToFront();
            panel.BackColor = Color.Gray;
            panel.Dock = DockStyle.Fill;
            panel.Opacity = 80;
            panel.Hide();
        }
    }
}
