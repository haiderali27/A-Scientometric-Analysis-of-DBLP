using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Threading;
using System.Diagnostics;
using DBLP;
using System.Windows.Forms.DataVisualization.Charting;

namespace DBLP
{

    public partial class frm_DBLP : Form
    {
        static string show_grid_view;
        static string server_name="dblb";
        parse x = new parse();
        DB x1 = new DB();
        public frm_DBLP()
        {
            InitializeComponent();
            
        }
        


        private void tabPage1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Thread thread1 = new Thread(() => x.article_parse());
            thread1.Start();
            
            if (thread1.IsAlive)
            {
                Application.UseWaitCursor = true;
            }
            






        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void browseDBLPFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oFD_MAIN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(oFD_MAIN.FileName);

                x.setxml(oFD_MAIN.FileName);




            }
        }

        private void fileAuthorsPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    x.setpath1(sfd.FileName + ".txt");

                }
            }

        }

        private void fileCitationsPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    x.setpath2(sfd.FileName + ".txt");


                }
            }
        }

        private void fileDetailsPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    x.setpath3(sfd.FileName + ".txt");

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (oFD_MAIN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(oFD_MAIN.FileName);

                x.setxml(oFD_MAIN.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    x.setpath1(sfd.FileName + ".txt");

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    x.setpath2(sfd.FileName + ".txt");
                    

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    x.setpath3(sfd.FileName + ".txt");

                }
            }
        }

        public void update()
        {
            try
            {
                Application.UseWaitCursor = true;
                DataAnalysis analyse = new DataAnalysis();
                analyse.analyse_publicationsandcitations_growth();
                analyse.author_growth();
                analyse.journalefficencyindex();
                analyse.relationalactivityindex();
                analyse.journal_impact_factor();
                analyse.jif();
                analyse.top10_articles();
                analyse.top10_author();
                analyse.summary();
            }
            finally
            {
                Application.UseWaitCursor = false;
            }
        }
        public void article_citation_growth()
        {
            DataAnalysis x = new DataAnalysis();
                        
            DataTable dt= x.show("article_citation_growth");

            int size = dt.Rows.Count;
            try
            {

                dgv_ACGrowth.DataSource = dt;
                
                double[] year = new double[size];
                double[] cyear = new double[size];
                double[] cited_counts = new double[size];
                double[] article_counts = new double[size];
                double[] average = new double[size];
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (i < size - 1)
                    {
                        year[i] = Convert.ToDouble(row["article_year"].ToString());


                        article_counts[i] = Convert.ToInt32(row["article_count"].ToString());

                        if (String.IsNullOrEmpty(row["cited_year"].ToString()) == false
                            )
                        {
                            cyear[i] = Convert.ToDouble(row["cited_year"].ToString());
                            average[i] = Convert.ToDouble(row["citation_per_artilce"].ToString());

                            if (cyear[i] >= 1993 && cyear[i] <= 2001)
                                cited_counts[i] = Convert.ToInt32(row["cited_count"].ToString()) * 50;

                            else if (cyear[i] >= 2002 && cyear[i] <= 2017)
                                cited_counts[i] = Convert.ToInt32(row["cited_count"].ToString()) * 500;

                        }
                    }

                    i++;

                }
                for (int j = 0; j < size; j++)
                {
                    cht_Publication.Series["Publications"].Points.AddXY(year[j], article_counts[j]);
                    cht_Publication.Series["Citations"].Points.AddXY(year[j], cited_counts[j]);

                    if (cyear[j] >= 1993 && cyear[j] <= 2001)
                        cited_counts[j] = cited_counts[j] / 50;
                    else if (cyear[j] >= 2002 && cyear[j] <= 2017)
                        cited_counts[j] = cited_counts[j] / 2500;

                    cht_Citations.Series["Citations"].Points.AddXY(year[j], cited_counts[j]);

                  
                }
            }catch(Exception e)
                {
                MessageBox.Show(e.Message);
            }
        }
        
        public void author_growth()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("article_author_count");

            int size = dt.Rows.Count;
            try
            {

                dgv_AuthorGrowth.DataSource = dt;

                double[] year = new double[size];
                double[] author = new double[size];

                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (i < size - 1)
                    {

                        year[i] = Convert.ToDouble(row["article_year"].ToString());
                        author[i] = Convert.ToDouble(row["author_count"].ToString());
                    }
                    i++;

                }

                    
                for (int j = 0; j < size; j++)
                {
                    cht_Authors.Series["Authors"].Points.AddXY(year[j], author[j]);
                }






            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void journal_efficency_index()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("journal_efficiency_index");

            int size = dt.Rows.Count;
            try
            {

                dgv_JEI.DataSource = dt;  

                string[] journal = new string[size];
                double[] jei = new double[size];
                double[] Ps = new double[size];
                double[] Cs = new double[size];
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    
                    jei[i] = Convert.ToDouble(row["JEI"].ToString());
                    Ps[i] = Convert.ToDouble(row["journal_count"].ToString());
                    Cs[i] = Convert.ToDouble(row["journal_cited_count"].ToString());
                    journal[i] = row["article_journal"].ToString();
                    
                    i++;

                }


                for (int j = 0; j < size; j++)
                {
                    cht_JEI.Series["JEI"].Points.AddXY(journal[j], jei[j]);
                    cht_JEIPC.Series["Publications"].Points.AddXY(journal[j], Ps[j]);
                    cht_JEIPC.Series["Citations"].Points.AddXY(journal[j], Cs[j]);

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void JIF()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("JIF");

            int size = dt.Rows.Count;
            try
            {

                dGV_JournalImpact.DataSource = dt;

                string[] journal = new string[size];
                double[] jif = new double[size];
                double[] Ps = new double[size];
                double[] Cs = new double[size];
                string[] year = new string[size];
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    jif[i] = Convert.ToDouble(row["JIF"].ToString());
                    Ps[i] = Convert.ToDouble(row["jpublication_count"].ToString());
                    Cs[i] = Convert.ToDouble(row["j_cited_count"].ToString());
                    journal[i] = row["article_journal"].ToString();
                    year[i] = row["article_year"].ToString();

                    i++;

                }
                lbl_JIFFF.Text = year[0];

                for (int j = 0; j < size; j++)
                {
                    cht_JournalImpact.Series["JIF"].Points.AddXY(journal[j], jif[j]);
                    cht_JournalImpact.Series["Citations"].Points.AddXY(journal[j], Cs[j]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void JIF(string year)
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.JIF(year);

            int size = dt.Rows.Count;
            try
            {

                dGV_JournalImpact.DataSource = dt;

                string[] journal = new string[size];
                double[] jif = new double[size];
                double[] Ps = new double[size];
                double[] Cs = new double[size];
               

                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    jif[i] = Convert.ToDouble(row["JIF"].ToString());
                    Ps[i] = Convert.ToDouble(row["jpublication_count"].ToString());
                    Cs[i] = Convert.ToDouble(row["j_cited_count"].ToString());
                    journal[i] = row["article_journal"].ToString();
                    

                    i++;
                }


                lbl_JIFFF.Text = year.ToString();
                foreach (var series in cht_JournalImpact.Series)
                {
                    series.Points.Clear();
                }



                for (int j = 0; j < size; j++)
                {
                    cht_JournalImpact.Series["JIF"].Points.AddXY(journal[j], jif[j]);
                    cht_JournalImpact.Series["Citations"].Points.AddXY(journal[j], Cs[j]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void relative_activity_index()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("relational_activity_index");
            
            int size = dt.Rows.Count;
            try
            {

                dgv_RAI.DataSource = dt;

                string[] journal = new string[size];
                double[] rai= new double[size];
                string year="2016";
                double var=0;
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    var += rai[i] = Convert.ToDouble(row["RAI"].ToString());
                    rai[i] = System.Math.Round(rai[i], 2);
                    journal[i] = row["article_journal"].ToString();
                    
                    i++;

                }
                lbl_RAII.Text = year;
                var = 100.0 - var;

                chtP_RAI.Series["RAI"]["PieLabelStyle"] = "Outside";
                chtP_RAI.Series["RAI"]["IsValueShownAsLabel"] = "true";
                chtP_RAI.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

                for (int j = 0; j < size; j++)
                {
                    cht_RAI.Series["RAI"].Points.AddXY(journal[j], rai[j]);
                    
                    chtP_RAI.Series["RAI"].Points.AddXY(journal[j], rai[j]);
                    
                }
                if (var > 0.0)
                {
                    chtP_RAI.Series["RAI"].Points.AddXY("Others", var);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void relative_activity_index(string year)
        {
            lbl_RAII.Text = year;
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.RAI(year);

            int size = dt.Rows.Count;
            try
            {

                dgv_RAI.DataSource = dt;

                string[] journal = new string[size];
                double[] rai = new double[size];

                int i = 0;
                double var = 0;
                foreach (DataRow row in dt.Rows)
                {

                    var += rai[i] = Convert.ToDouble(row["RAI"].ToString());
                    rai[i] = System.Math.Round(rai[i], 2);
                    journal[i] = row["article_journal"].ToString();

                    i++;

                }
                var = 100 - var;
                foreach (var series in chtP_RAI.Series)
                {
                    series.Points.Clear();
                }

                foreach (var series in cht_RAI.Series)
                {
                    series.Points.Clear();
                }


                chtP_RAI.Series["RAI"]["PieLabelStyle"] = "Outside";
                chtP_RAI.Series["RAI"]["IsValueShownAsLabel"] = "true";
                chtP_RAI.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

                for (int j = 0; j < size; j++)
                {
                    cht_RAI.Series["RAI"].Points.AddXY(journal[j], rai[j]);
                    chtP_RAI.Series["RAI"].Points.AddXY(journal[j], rai[j]);
                   
                }

                if (var > 0.0)
                {
                    chtP_RAI.Series["RAI"].Points.AddXY("Others", var);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void coauthorshipindex()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("coauthorship_index");

            int size = dt.Rows.Count;
            try
            {

                dgv_CAI.DataSource = dt;

                string[] authored = new string[size];
                double[] Cs = new double[size];
                double[] Ps = new double[size];


                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    Ps[i] = Convert.ToDouble(row["publication_count"].ToString());
                    Cs[i] = Convert.ToDouble(row["cited_count"].ToString())*10;
                    authored[i] = row["authored"].ToString();

                    i++;

                }


                for (int j = 0; j < size; j++)
                {
                    cht_Co.Series["Publications"].Points.AddXY(authored[j], Ps[j]);
                    cht_Co.Series["Citations"].Points.AddXY(authored[j], Cs[j]);
                    Cs[j] /= 10;
                    cht_CoC.Series["Citations"].Points.AddXY(authored[j], Cs[j]);
                }

                i = 0;
                dt = x.co_author();
                size = dt.Rows.Count;
                string[] journals = new string[size];
                string[] author = new string[size];
                double[] P1s = new double[size];
                foreach (DataRow row in dt.Rows)
                {

                    P1s[i] = Convert.ToDouble(row["publication_count"].ToString());
                    journals[i]= row["article_journal"].ToString();
                    author[i] = row["authored"].ToString();

                    i++;

                }


                for (int j = 0; j < size; j++)
                {
                    cht_CoCC.Series["Publications"].Points.AddXY(journals[j], P1s[j]);
                    cht_CoCC.Series["Publications"].Points[j].Label = author[j];
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }
        public void journal_impact_factor()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("journal_impact_factor");

            int size = dt.Rows.Count;
            try
            {

                dgv_JIF.DataSource = dt;

                string[] journal = new string[size];
                double[] Ps = new double[size];
                double[] Cs = new double[size];
                double[] JIF = new double[size];
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    JIF[i] = Convert.ToDouble(row["JIF"].ToString());
                    journal[i] = row["article_journal"].ToString();
                    Ps[i] = Convert.ToDouble(row["jpublication_count"].ToString());
                    Cs[i] = Convert.ToDouble(row["j_cited_count"].ToString());

                    i++;

                }


                for (int j = 0; j < size; j++)
                {
                    cht_JIF.Series["JIF"].Points.AddXY(journal[j], JIF[j]);
                    cht_JIFF.Series["Publications"].Points.AddXY(journal[j], Ps[j]);
                    cht_JIFF.Series["Citations"].Points.AddXY(journal[j], Cs[j]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        public void top_ten_authors()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("top_ten_authors");

            int size = dt.Rows.Count;
            try
            {


                dgv_TopAuthors.DataSource = dt;

                string[] author = new string[size];
                double[] Ps = new double[size];
                double[] Cs = new double[size];
                double[] CoPs= new double[size];

                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    CoPs[i] = Convert.ToDouble(row["cite_per_publication"].ToString());
                    Cs[i] = Convert.ToDouble(row["cited_count"].ToString());
                    Ps[i] = Convert.ToDouble(row["publication_count"].ToString());
                    author[i] = row["article_author"].ToString();

                    i++;

                }


                for (int j = 0; j < size; j++)
                {
                    cht_Author.Series["Publications"].Points.AddXY(author[j], Ps[j]);
                    cht_Author.Series["Citations"].Points.AddXY(author[j],Cs[j]);
                    cht_Author.Series["CitationOverPublication"].Points.AddXY(author[j], CoPs[j]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void top_ten_articles()
        {
            DataAnalysis x = new DataAnalysis();

            DataTable dt = x.show("top_ten_articles");

            int size = dt.Rows.Count;
            try
            {

                dgv_TopArticles.DataSource = dt;

                string[] title = new string[size];
                double[] cited_count = new double[size];

                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    
                    
                        title[i] = (row["article_title"].ToString());
                        cited_count[i] = Convert.ToDouble(row["citation_count"].ToString());
                    
                    i++;

                }


                for (int j = 0; j < size; j++)
                {
                    cht_Articles.Series["Citations"].Points.AddXY(title[j], cited_count[j]);
                }






            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            

            article_citation_growth();
            author_growth();
            journal_efficency_index();
            relative_activity_index();
            coauthorshipindex();
            journal_impact_factor();
            top_ten_authors();
            top_ten_articles();
            JIF();
            DataAnalysis x = new DataAnalysis();
            dgv_Summary.DataSource = x.summary();

            // TODO: This line of code loads data into the 'dblbDataSet3.article_citation_growth' table. You can move, or remove it, as needed.
            /*





             */




        }

        private void btn_CS_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Server.Text != "" && txt_Database.Text != "")
                {
                    server_name = txt_Server.ToString();
                    x1.set_CS(txt_Server.Text.ToString(), txt_Database.Text.ToString());
                    lbl_Require.Text = "";
                }

                else if (txt_Server.Text == "" || txt_Database.Text == "")
                {
                    Exception x = new Exception("Please Write the name of 'Server' and 'Database'");
                    throw x;
                }
            }catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (rdo_AUTHOR.Checked)
            {
                if (DB.check(show_grid_view) > 0)
                {
                    MessageBox.Show("Table has records already");
                }
                else
                {
                    var thread = new Thread(() => DB.insert_article_author_record());
                    thread.Start();
                }
            }
            else if (rdo_CITATION.Checked)
            {
                if(DB.check(show_grid_view)>0)
                {
                    MessageBox.Show("Table has records already");
                }
                else
                {
                    var thread = new Thread(() => DB.insert_article_citation_record());
                    thread.Start();
                }
            }
            else if (rdo_DETAILS.Checked)
            {
                if (DB.check(show_grid_view) > 0)
                {
                    MessageBox.Show("Table has records already");
                }
                else
                {
                    var thread = new Thread(() => DB.insert_article_details_record());
                    thread.Start();
                }
            }
            else
                MessageBox.Show("Please, Select Some Option");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (rdo_AUTHOR.Checked)
            {
                if (DB.check(show_grid_view) == 0)
                {
                    MessageBox.Show("Table has no records already");
                }
                else
                {
                    var thread = new Thread(() => x1.delete_article_author_record());
                    thread.Start();
                }
            }
            else if (rdo_CITATION.Checked)
            {
                if (DB.check(show_grid_view) == 0)
                {
                    MessageBox.Show("Table has no records already");
                }
                else
                {
                    var thread = new Thread(() => x1.delete_article_citation_record());
                    thread.Start();
                }
            }
            else if (rdo_DETAILS.Checked)
            {
                if (DB.check(show_grid_view) == 0)
                {
                    MessageBox.Show("Table has no records already");
                }
                else
                {
                    var thread = new Thread(() => x1.delete_article_details_record());
                    thread.Start();
                }
            }
            else
                MessageBox.Show("Please, Select Some Option");
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        public void point()
        {
            if (rdo_AUTHOR.Checked)
                show_grid_view = "article_author_record";
            else if (rdo_CITATION.Checked)
                show_grid_view = "article_citation_record";
            else if (rdo_DETAILS.Checked)
                show_grid_view = "article_details_record";
        }

        private void rdo_AUTHOR_CheckedChanged(object sender, EventArgs e)
        {
            point();
        }

        private void rdo_CITATION_CheckedChanged(object sender, EventArgs e)
        {
            point();
        }

        private void rdo_DETAILS_CheckedChanged(object sender, EventArgs e)
        {
            point();
        }

        private void tSB_Browse_Click(object sender, EventArgs e)
        {

        }

        private void dgv_ACGrowth_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void btn_Update_ACGrowth_Click(object sender, EventArgs e)
        {
           
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_Update_Author_Growth_Click(object sender, EventArgs e)
        {
         
        }

        private void btn_Update_JEI_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Update_RAI_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Update_CAI_Click(object sender, EventArgs e)
        {
          
        }

        private void btn_Update_JIF_Click(object sender, EventArgs e)
        {
            DataAnalysis x = new DataAnalysis();
            x.journal_impact_factor();
        }

        private void btn_Update_TopTenAuthors_Click(object sender, EventArgs e)
        {
           
        }

        private void btn_Update_TopTenArticles_Click(object sender, EventArgs e)
        {
          
        }

        private void btn_Show_AuthorGrowth_Click(object sender, EventArgs e)
        {
            
          
           
        }

       

        private void btn_Show_JEI_Click(object sender, EventArgs e)
        {
            
           
        }

        private void btn_Show_RAI_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Show_CAI_Click(object sender, EventArgs e)
        {
           
            
        }

        private void btn_Show_JIF_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Show_TopTenAuthors_Click(object sender, EventArgs e)
        {

        }

        private void btn_Show_TopTenArticles_Click(object sender, EventArgs e)
        {
            

          
        }

        private void btn_Drop_Click(object sender, EventArgs e)
        {
            x1.drop_all_tables();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            if(show_grid_view=="article_details_record")
            x1.create_table_article_details_record();

           else if (show_grid_view == "article_citation_record")
                x1.create_table_article_citation_record();
            else if(show_grid_view=="article_details_record")
                x1.create_table_article_author_record();
               
            else
                MessageBox.Show("Please, Select Some Option");
        }


        private void btn_Show_Summary_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will update all analysis and it will take minutes", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var thread = new Thread(update);
                thread.Start();
            }
            else if(dialogResult == DialogResult.No)
            {
                MessageBox.Show("No Anaylsis is performed");
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void txt_Database_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (oFD_MAIN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(oFD_MAIN.FileName);

                x.setpath1(oFD_MAIN.FileName);
            }
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (oFD_MAIN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(oFD_MAIN.FileName);

                x.setpath2(oFD_MAIN.FileName);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (oFD_MAIN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(oFD_MAIN.FileName);

                x.setpath3(oFD_MAIN.FileName);
            }

        }

       

        

        

        private void btn_Search_Click(object sender, EventArgs e)
        {

            Application.UseWaitCursor = true;
            dgv_DISPLAY.DataSource = x1.search(txt_Search, rdo_Key, rdo_Name, rdo_Title, rdo_Journal, rdo_Year, rdo_Cite);
            
            
             
            

        }

        private void chk_YEAR_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cht_Citations_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_3(object sender, EventArgs e)
        {
            relative_activity_index(txt_RAI.Text.ToString());
        }

        private void cht_Citations_Click_1(object sender, EventArgs e)
        {

        }

        private void cht_Author_Click(object sender, EventArgs e)
        {

        }

        private void btn_JIF_Click(object sender, EventArgs e)
        {
            
            JIF(txt_JIF.Text.ToString());
        }

        private void dgv_DISPLAY_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbl_CP_Click(object sender, EventArgs e)
        {

        }

        private void tP_RAI_Click(object sender, EventArgs e)
        {

        }

        private void chtP_RAI_Click(object sender, EventArgs e)
        {

        }

        private void tP_JIF_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }

}




/// <summary>
/// //////////////////////////////////////Parse Data //////////////////////////////////////////////
/// </summary>
    public class parse :remove
{
    static string xml_File;
    static string filename1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\DBLP\\a1.txt";
    static string filename2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +"\\DBLP\\a2.txt";
    static string filename3 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +"\\DBLP\\a3.txt";
    string cite_record, author_record, article_detail;
    int count=0;
    string author, title, journal, year;
    StreamWriter writer0, writer1, writer2;
    StringBuilder xml;
    XmlReaderSettings settings;
    XmlReader reader;
    XElement x;
    
    public static string getpath1()
    {
  
     
        return filename1;
    }
    public static string getpath2()
    {
       
        
        
        return filename2;
    }
    public static string getpath3()
    {
     
        
        return filename3;
    }
    public void setxml(string a)
    {
        xml_File = a;
    }
    public void setpath1(string a)
    {
        filename1 = @a;
    }
    public void setpath2(string a)
    {
        filename2 = @a;
    }
    public void setpath3(string a)
    {
        filename3 = @a;
    }
    public void countarticle()
    {
        var checkpoint = false;
        Stopwatch stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            if (xml_File == null)
            {
                Exception e = new Exception("Please Select path for dblp.xml file");
                throw e;
            }

            StringBuilder xml1 = new StringBuilder();
            XmlReaderSettings settings1 = new XmlReaderSettings();
            settings1.DtdProcessing = DtdProcessing.Parse;
            XmlReader reader = XmlReader.Create(@xml_File, settings1);

            count = 0;
            while (reader.ReadToFollowing("article"))
            {
                count++;
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            checkpoint = true;
        }
        finally
        {
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            if (checkpoint == false)
            {
                MessageBox.Show("Number of article records are" + count + "and time it took is to" +
                    "count is" + elapsedTime);
            }
            Application.UseWaitCursor = false;
        }
        
    }
    public int getcount()
    {
        return count;
    }  
    public void article_parse() // this function extract data out of xml file
    {
        var checkpoint = false;
        int record_number = 0;
        Stopwatch stopwatch = new Stopwatch();
        try
        {
            if (xml_File == null)
            {
                Exception e = new Exception("Please Select path for dblp.xml file");
                throw e;
            }
            stopwatch.Start();
            xml = new StringBuilder();
            settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            reader = XmlReader.Create(@xml_File, settings);


            writer0 = File.CreateText(filename1);
            writer0.Flush();
            writer0.Close();

            writer1 = File.CreateText(filename2);
            writer1.Flush();
            writer1.Close();


            writer2 = File.CreateText(filename3);
            writer2.Flush();
            writer2.Close();


            writer0 = new StreamWriter(filename1, true, Encoding.UTF8, 65536);
            writer1 = new StreamWriter(filename2, true, Encoding.UTF8, 65536);
            writer2 = new StreamWriter(filename3, true, Encoding.UTF8, 65536);


            reader.ReadToFollowing("article");
            do
            {
                record_number++;


                x = (XElement)XNode.ReadFrom(reader);

                for (int i = 0; i < x.Descendants("author").Count(); i++)
                {
                    author = RemoveDiacritics(x.Descendants("author").ElementAt(i).Value);

                    author_record = x.Attribute("key").Value + "_rno_" +
                    record_number.ToString() + "\t" + author +
                    Environment.NewLine;
                    writer0.Write(author_record);


                }

                for (int i = 0; i < x.Descendants("cite").Count(); i++)
                {
                    if (x.Descendants("cite").ElementAt(i).Value != "...")
                    {
                        cite_record = x.Attribute("key").Value + "_rno_" +
                        record_number.ToString() + '\t' + x.Descendants("cite").ElementAt(i).Value +
                        Environment.NewLine;
                        writer1.Write(cite_record);


                    }

                }

                year = "";
                title = "";
                journal = "";
                if (x.Descendants("year").Count() < 1)
                {
                    year = "null";
                }
                else
                {
                    year = x.Element("year").Value;
                }

                if (x.Descendants("title").Count() < 1)
                {
                    title = "null";
                }
                else
                {
                    title = RemoveDiacritics(x.Element("title").Value);

                }
                if (x.Descendants("journal").Count() < 1)
                {
                    journal = "null";
                }
                else
                {
                    journal = RemoveDiacritics(x.Element("journal").Value);
                }
                article_detail = x.Attribute("key").Value + "_rno_" +
                record_number.ToString() + '\t' + year + '\t' + title
                + '\t' + journal + Environment.NewLine;


                writer2.Write(article_detail);

                
                
                if (record_number>1499999)
                {
                    writer0.Flush();
                    writer1.Flush();
                    writer2.Flush();
                }
                
            } while (
            reader.ReadToFollowing("article"));
            writer0.Close();
            writer1.Close();
            writer2.Close(); 

        }
    
     catch (Exception e)
      {
            MessageBox.Show(e.Message);
            checkpoint = true;
      }
        finally
        {
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            if (checkpoint != true)
            {
                MessageBox.Show("Data Have Been Parsed Successfully in time:"+elapsedTime);
                Application.UseWaitCursor = false;
            }
            else
            {
                MessageBox.Show("Data couldn't be parsed, time:" + elapsedTime);
                Application.UseWaitCursor = false;
                    
            }
                
        }
        

    }

    

};

    public partial class remove
{
    private readonly static Regex nonSpacingMarkRegex =
new Regex(@"\p{Mn}", RegexOptions.Compiled);

    public static string RemoveDiacritics(string text)
    {
        if (text == null)
            return string.Empty;

        var normalizedText =
            text.Normalize(NormalizationForm.FormD);
        string string_return = nonSpacingMarkRegex.Replace(normalizedText, string.Empty);

        if (string_return.Contains('Ø'))
            string_return = string_return.Replace("Ø", "O");


        if (string_return.Contains("ø"))
            string_return = string_return.Replace("ø", "o");


        if (string_return.Contains("Ð"))
            string_return = string_return.Replace("Ð", "D");




        return string_return;
    }
};
/// <summary>
/// //////////////////////////////////// Database//////////////////////////////////////////////////////
/// </summary>
    public partial class DB:parse
{
    static string connection_string = @"Data Source=" + "desktop" + ";Initial Catalog=" + "dblb" + ";Integrated Security=True;Pooling=False";
    static SqlConnection con = new SqlConnection(connection_string);
    static SqlDataAdapter da;
    static DataTable dt;
   

    public DataTable get_DataTable()
    {
        return dt;
    }

    public void set_CS(string a,string b)
    {
        connection_string = @"Data Source=" + a + ";Initial Catalog=" + b + ";Integrated Security=True;Pooling=False";
    }
    public static string get_CS()
    {
        return connection_string;
    }
    public void drop_all_tables()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"
IF OBJECT_ID('article_author_record','U') IS NOT NULL 
Drop table article_author_record
IF OBJECT_ID('article_citation_record','U') IS NOT NULL 
Drop table article_citation_record
IF OBJECT_ID('article_details_record','U') IS NOT NULL 
Drop table article_details_record
";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }
    public static void insert_article_author_record()
    {
        try
        {
            Application.UseWaitCursor = true;
           
            con.Open();
           
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = @"bulk insert article_author_record
from  'C:\Users\SayHelloxXx\Documents\DBLP\a1.txt' with (fieldterminator = '\t', rowterminator = '\n');";
                command.CommandTimeout = 180;
                command.ExecuteNonQuery();
                da = new SqlDataAdapter(command);
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
            Application.UseWaitCursor = false;
        }

    }
    public static int check(string a)
    {
        int sqlresult=0;
        try
        {

            con.Open();
            using (SqlCommand sqlCommand = new SqlCommand("SELECT count(1) from " + a + "", con))
            {
                sqlresult = (int)sqlCommand.ExecuteScalar();
            }
        }
        
        finally
        {
            con.Close();
        }

        return sqlresult;

        
        }
    public static void insert_article_citation_record()
    {
        try
        {
            Application.UseWaitCursor = true;
           
            con.Open(); 
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;


                command.CommandText = @"bulk insert article_citation_record
from 'C:\Users\SayHelloxXx\Documents\DBLP\a2.txt'  with (fieldterminator = '\t', rowterminator = '\n');";

                command.ExecuteNonQuery();
                da = new SqlDataAdapter(command);
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
            Application.UseWaitCursor = false;
        }

    }
    public  static void insert_article_details_record()
    {
        try
        {
            Application.UseWaitCursor = true;
            con.Open();
           
            
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = @"bulk insert article_details_record
from 'C:\Users\SayHelloxXx\Documents\DBLP\a3.txt' with (fieldterminator = '\t', rowterminator = '\n');";
                command.CommandTimeout = 180;
                command.ExecuteNonQuery();
                da = new SqlDataAdapter(command); 
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
            Application.UseWaitCursor = false;
        }
    }

    public static void create_search_table()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"
IF OBJECT_ID('search_table','U') IS NOT NULL 
drop table search_table
select A.article_key,cited_key,article_author,article_title,article_year,article_journal into search_table
 from 
(
select B.article_key,article_journal,article_year,article_title,article_author from 
article_author_record A,article_details_record B 
where A.article_key= B.article_key) as A

LEFT Join article_citation_record C
ON A.article_key= C.article_key;
";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }


    public void delete_article_author_record()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"delete from article_author_record";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }
    public void delete_article_citation_record()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"delete from article_citation_record";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }
    public void delete_article_details_record()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"delete from article_details_record";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }
    public void create_table_article_details_record()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"CREATE TABLE [dbo].[article_details_record]
(
	[article_key] VARCHAR(100) NOT NULL PRIMARY KEY, 
    [article_year] VARCHAR(4) NULL, 
    [article_title] VARCHAR(3000) NULL, 
    [article_journal] VARCHAR(200) NULL
)

";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }

    public void create_table_article_citation_record()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"

CREATE TABLE [dbo].[article_citation_record] (
    [article_key] VARCHAR (100) NOT NULL,
    [cited_key]   VARCHAR (100) NOT NULL, 
CONSTRAINT [FK_article_citation_record_ToTable] FOREIGN KEY ([article_key]) REFERENCES [article_details_record]([article_key]), 
    CONSTRAINT [FK_article_citation_record_ToTable_1] FOREIGN KEY ([cited_key]) REFERENCES [article_details_record]([article_key])
    
);




";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }

    public DataTable search(TextBox txt_search,RadioButton key,RadioButton author, RadioButton title, RadioButton journal, RadioButton year, RadioButton cite)
    {
        Application.UseWaitCursor = true;
        try
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            if (key.Checked == true)
                cmd.CommandText = @"select * from search_table  where article_key = '"+ txt_search.Text+"' ";

            else if (author.Checked)
                cmd.CommandText = @"select * from search_table  where article_author = '" + txt_search.Text + "' ";

            else if (year.Checked)
                cmd.CommandText = @"select * from search_table  where article_year ='"+txt_search.Text+"';";

            else if (journal.Checked)
                cmd.CommandText = @"select * from search_table  where article_journal = '" + txt_search.Text + "'";

            else if (cite.Checked)
                cmd.CommandText = @"select * from search_table  where cited_key = '" + txt_search.Text + "'";

            else if (title.Checked)
                cmd.CommandText = @"select * from search_table  where article_title = '" + txt_search.Text + "'";


            da = new SqlDataAdapter(cmd.CommandText, con);

            dt = new DataTable();
            da.Fill(dt);

            


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
            Application.UseWaitCursor = false;
        }
        return dt;
    }


    public void create_table_article_author_record()
    {
        try
        {
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = @"

CREATE TABLE [dbo].[article_author_record] (
    [article_key]    VARCHAR (100) NOT NULL,
    [article_author] VARCHAR (100) NOT NULL,
    CONSTRAINT [FK_article_author_record] FOREIGN KEY ([article_key]) REFERENCES [dbo].[article_details_record] ([article_key])
);



";
            command.CommandTimeout = 180;
            command.ExecuteNonQuery();
            da = new SqlDataAdapter(command);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }
};



/// <summary>
/// ///////////////////////////Analysis/////////////////////////////////////////////////////////////
/// </summary>
public class DataAnalysis : DB
{


    static string connection_string = DB.get_CS();
    static SqlConnection con = new SqlConnection(connection_string);
    static SqlDataAdapter dataadapter;
    static DataTable datatable;
    
    public void analyse_publicationsandcitations_growth()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/*publication_citation_growth*/
IF OBJECT_ID('tempdb..#article_publication_count') IS NOT NULL 
drop table #article_publication_count
select DISTINCT article_year,count(article_year )as article_count into #article_publication_count
from article_details_record
where article_year>=(select min(article_year)
from article_details_record) and article_year<(select MAX(article_year) from article_details_record)
group by article_year
Union all
select 'Total' article_year,count(article_year) from article_details_record
order by article_year;
IF OBJECT_ID('tempdb..#article_citation_count') IS NOT NULL 
drop table #article_citation_count
select DISTINCT article_year as cited_year,count(article_year) as cited_count into #article_citation_count 
from article_details_record A,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key
group by article_year
union all
select 'Total' article_year,count(article_year) from article_details_record A,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key 
order by article_year;

IF OBJECT_ID('tempdb..#A') IS NOT NULL 
drop table #A
select *,convert(decimal(7,3),(article_count * 100.0 / (Select SUM(article_count) From #article_publication_count where article_year!='Total'))) as article_percentage, convert(decimal(10,3),article_count*100.0/LAG(article_count,1) over (order by article_year)-100) as article_growth
into #A
from #article_publication_count
group by article_year,article_count;


IF OBJECT_ID('tempdb..#B') IS NOT NULL 
drop table #B
select *,convert(decimal(7,4),(cited_count * 100.0 / (Select SUM(cited_count) From #article_citation_count where cited_year!='Total'))) as citation_percentage,convert(decimal(10,3),cited_count*100.0/ LAG(cited_count,1) over (order by cited_year)-100.0)  as citation_growth
into #B
from #article_citation_count
group by cited_year,cited_count;

IF OBJECT_ID('article_citation_growth','U') IS NOT NULL 
drop table article_citation_growth
select *,convert(decimal(5,4),cited_count*1.0/article_count) as citation_per_artilce 
into article_citation_growth
from #A A LEFT JOIN #B B
ON A.article_year=B.cited_year;

drop table #A;
drop table #B;
drop table #article_citation_count;
drop table #article_publication_count;

update article_citation_growth
set citation_growth=NULL,article_growth= NULL 
where article_year='Total' and cited_year='Total';
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);

            
            

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }
    public void author_growth()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/*author growth*/
IF OBJECT_ID('tempdb..#countt') IS NOT NULL 
drop table #countt
select COUNT( distinct article_author) as author_count,article_year into #countt
from(
select min (article_year) as article_year, article_author
from article_author_record inner join article_details_record on article_details_record.article_key=article_author_record.article_key
group by article_author) as yearwise_author
group by article_year
order by article_year;
IF OBJECT_ID('tempdb..#article_author_count') IS NOT NULL 
drop table #article_author_count 
select * into #article_author_count
from #countt
union all
select sum(author_count) ,'Total' article_year from #countt
drop table #countt
IF OBJECT_ID('article_author_count','U') IS NOT NULL 
drop table article_author_count 
select article_year,author_count,convert(decimal(10,3),(author_count)*100.0/LAG(author_count,1) over (order by article_year)-100) as author_growth into article_author_count
from #article_author_count
group by article_year,author_count;

drop table #article_author_count;

update article_author_count 
set author_growth=NULL
where article_year='Total';

";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);



        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }

    public void journalefficencyindex()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/*journal efficency index */

IF OBJECT_ID('journal_efficiency_index','U') IS NOT NULL 
drop table journal_efficiency_index
select *,
convert(decimal(7,4),percentage_journal_cited_count/percentage_journal_count) as JEI
into journal_efficiency_index
from(
select A.article_journal,
journal_count,
journal_cited_count,
convert(decimal(5,4),journal_count*100.0/(select sum(journal_count) from (select article_journal,count(article_journal)as journal_count from article_details_record
group by article_journal) as C )) as percentage_journal_count,
convert(decimal(7,4),journal_cited_count*100.0/(select sum(journal_cited_count) from (select article_journal,count(article_journal) as journal_cited_count
from article_details_record A ,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))= B.cited_key
group by article_journal) as D)) as percentage_journal_cited_count,
convert(decimal(7,4),journal_cited_count*1.0/journal_count) as ACP

from
(select article_journal,count(article_journal) as journal_cited_count
from article_details_record A ,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))= B.cited_key
group by article_journal) as A 
,
(select article_journal,count(article_journal)as journal_count from article_details_record
group by article_journal) as B

where A.article_journal=B.article_journal 
group by journal_count,A.article_journal,journal_cited_count) as A

order by JEI Desc;

";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }

    public void relationalactivityindex()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/*RAI of a journal*/
IF OBJECT_ID('relational_activity_index','U') IS NOT NULL 
drop table relational_activity_index
select article_journal,article_year,RAI into relational_activity_index
from(
select article_year,count_journal as all_year_count,article_journal,count_spec as specific_count,convert(decimal(11,4),count_spec*100.0/count_journal) as RAI from
(select distinct A.article_year,count_journal,article_journal,count(article_journal) as count_spec
from
(
select min(article_year) as article_year,count(article_journal) as count_journal 
from article_details_record
where article_year!='NULL'
group by article_year) as A,article_details_record B
where A.article_year=B.article_year 
group by A.article_year,A.count_journal,B.article_journal) as A) as A
order by article_year;
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);





        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }
    public void co_authorship_index()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/*coauthorship index*/
/*coauthorship index*/
IF OBJECT_ID('coauthorship_index','U') IS NOT NULL 
drop table coauthorship_index
select A.authored,cited_authored_count as cited_count,publication_authored_count as publication_count,convert(decimal(5,4),cited_authored_count*1.0/publication_authored_count) as citation_per_publication 
,A.article_journal
into coauthorship_index
from
(select min(authored) as authored,count(authored) as publication_authored_count,article_journal from(
select 
             CASE 
                  WHEN author_count = 1 
                     THEN 'Single' 
			      When author_count=2
				     Then 'Two'
			      When author_count >2 and author_count<11
				     Then 'Multiple'
                  ELSE 'Mega' 
             END AS authored,

article_journal
from(
select article_journal,author_count
from(
select A.article_key,author_count,article_journal,article_year
from
(select min(article_key) as article_key,count(article_key) as author_count
from article_author_record
group by article_key) as A,article_details_record B
where A.article_key=B.article_key) as A
group by article_journal,author_count,article_year) as A) as A
group by authored,article_journal) as A left join

(select min(authored) as authored,count(authored) as cited_authored_count,article_journal from(
select 
             CASE 
                  WHEN author_count = 1 
                     THEN 'Single' 
			      When author_count=2
				     Then 'Two'
			      When author_count >2 and author_count<11
				     Then 'Multiple'               
                  ELSE 'Mega' 
             END AS authored,

article_journal
from(
select article_journal,author_count
from(
select A.article_key,author_count,article_journal
from
(select min(article_key) as article_key,count(article_key) as author_count
from article_author_record
group by article_key) as A,article_details_record B,article_citation_record C
where A.article_key=B.article_key AND SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=C.cited_key AND SUBSTRING(B.article_key,0,CHARINDEX('_',B.article_key,0))=C.cited_key) as A
group by article_journal,author_count) as A) as A
group by authored,article_journal)as B
ON A.authored=B.authored and A.article_journal=B.article_journal  ;
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }


    public void journal_impact_factor()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/* JIF */
IF OBJECT_ID('journal_impact_factor','U') IS NOT NULL 
drop table journal_impact_factor
select top(10) A.article_journal,A.article_year,count_journal as jpublication_count,j_cited_count,convert(decimal(7,4),j_cited_count*1.0/count_journal) as JIF
into journal_impact_factor 
from
(select article_journal,min(article_year) as article_year,count(article_journal) as count_journal
from article_details_record
group by article_year,article_journal) as A,
(select article_journal, min(article_year) as article_year,count(article_journal) as j_cited_count
from article_details_record A,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key
group by article_journal) as B
where A.article_journal=B.article_journal and A.article_year=B.article_year
order by JIF desc;
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }


    public void top10_author()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/*top 10 authors*/
IF OBJECT_ID('tempdb..#A') IS NOT NULL 
drop table #A
(select distinct article_author, count(article_author) as cited_count into #A
from article_author_record A,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key
group by article_author)

IF OBJECT_ID('tempdb..#B') IS NOT NULL 
drop table #B
(select distinct article_author, count(article_author) as publication_count into #B
from article_author_record A,article_details_record B
where A.article_key=B.article_key
group by article_author)

IF OBJECT_ID('top_ten_authors','U') IS NOT NULL 
drop table top_ten_authors
select A.article_author,cited_count,publication_count,convert(decimal(8,5),(cited_count*1.0/publication_count))as cite_per_publication
into top_ten_authors
from #A A,#B B 
where A.article_author=B.article_author
order by cite_per_publication desc;
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }



    public void top10_articles()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/* top 10 articles*/
IF OBJECT_ID('top_ten_articles','U') IS NOT NULL 
drop table top_ten_articles
select distinct top 10 article_title,count(article_title) as citation_count
into top_ten_articles
from
(select article_title
from article_details_record A,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key)as A
group by article_title
order by citation_count desc;
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }

    public void jif()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"/* JIF Two Yearly*/
IF OBJECT_ID('tempdb..#A') IS NOT NULL
drop table #A
select article_journal,article_year,jpublication_count,COALESCE(j_cited_count,0)as j_cited_count into #A
from
(select  A.article_journal,A.article_year,count_journal as jpublication_count,j_cited_count
from
(select article_journal,min(article_year) as article_year,count(article_journal) as count_journal
from article_details_record
group by article_year,article_journal) as A
LEFT JOIN
(select article_journal, min(article_year) as article_year,count(article_journal) as j_cited_count
from article_details_record A,article_citation_record B
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key
group by article_journal) as B
ON A.article_journal=B.article_journal and A.article_year=B.article_year

order by article_journal,article_year
OFFSET 0 rows)
as A

IF OBJECT_ID('JIF','U') IS NOT NULL 
drop table JIF
select *,
Convert(dec(8,5),
thisandlast_cited*1.0/thisandlast_pub) as JIF
into JIF
from
(select *,
jpublication_count+LAG(jpublication_count,1) over (Partition by article_journal order by article_year) As thisandlast_pub
,
COALESCE( 
j_cited_count+LAG(j_cited_count,1) over (Partition by article_journal order by article_year),0) as thisandlast_cited
from
#A)as A
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }
    public DataTable summary()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"
select * from summary
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return datatable;
    }

    public void update_summary()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            command2.CommandText = @"
select Field,Total_Count into summary 
from(
select article_author as Field, Total_Count from(
select 'Authors' article_author, count(article_author) as Total_Count from(
select distinct article_author
from article_author_record) as A)as A
union all
select article_key as Field, Total_Count from(
select 'Articles' article_key,count(article_key) as Total_Count
from article_details_record) as A
union all
select cited_key, Total_Count from(
select 'Citations' cited_key, count(cited_key) as Total_Count
from article_citation_record B,article_details_record A
where SUBSTRING(A.article_key,0,CHARINDEX('_',A.article_key,0))=  B.cited_key)
as A) as A
";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
       
    }

    public DataTable RAI(string a)
    {
       
try{
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;


            command2.CommandText = @"
  select top (4) article_journal, RAI
from relational_activity_index
where article_year = '"+a+"'order by RAI desc";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

        return datatable;
    }
    public DataTable JIF(string a)
    {

        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;


            command2.CommandText = @"
  select * from JIF
where JIF>0.0
and article_year = '" + a + "'order by article_year";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }

        return datatable;
    }
    public DataTable co_author()
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;


            command2.CommandText = @"
  select top (5) * from(
select min(authored) as authored, sum(publication_count) as publication_count, min(article_journal) as article_journal
from coauthorship_index
group by authored, article_journal
) as A
order by publication_count desc;";
            command2.CommandTimeout = 180;
            command2.ExecuteNonQuery();
            dataadapter = new SqlDataAdapter(command2);
            datatable = new DataTable();
            dataadapter.Fill(datatable);
        }catch (Exception ex){
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return datatable;

    }
    public DataTable show(string tablename)
    {
        try
        {
            con.Open();



            SqlCommand command2 = con.CreateCommand();
            command2.CommandType = CommandType.Text;

            if (tablename == "article_citation_growth")
            {
                command2.CommandText = @"
select *
from
article_citation_growth where article_year>'1992'";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);

            }
            if (tablename == "article_author_count")
            {
                command2.CommandText = @"
select *
from
article_author_count
where article_year>'1992';";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }
            if (tablename == "journal_efficiency_index")
            {
                command2.CommandText = @"
select top (5) *
from
journal_efficiency_index
order by JEI desc;";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }

            if (tablename == "relational_activity_index")
            {
                command2.CommandText = @"
select top (4) article_journal, RAI
from relational_activity_index
where article_year = '2016' order by RAI desc
";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }

            if (tablename == "journal_impact_factor")
            {
                command2.CommandText = @"
select top (5) *
from
journal_impact_factor 
Order by JIF desc;";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }

            if (tablename == "coauthorship_index")
            {
                command2.CommandText = @"
select min(authored) as authored,sum(cited_count) as cited_count,sum(publication_count)as publication_count,CONVERT(decimal(5,4),sum(cited_count)*1.0/sum(publication_count)) as CiteOverPubl
from coauthorship_index
group by authored";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }

            if (tablename == "top_ten_authors")
            {
                command2.CommandText = @"
select top(5) *
from
top_ten_authors
where publication_count>10
order by cite_per_publication desc;";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }
            if (tablename == "JIF")
            {
                command2.CommandText = @"
select * from JIF
where JIF>0.0
and article_year =2016
order by article_year";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }

            if (tablename == "top_ten_articles")
            {
                command2.CommandText = @"
select top(5) *
from
top_ten_articles;";
                command2.CommandTimeout = 180;
                command2.ExecuteNonQuery();
                dataadapter = new SqlDataAdapter(command2);
                datatable = new DataTable();
                dataadapter.Fill(datatable);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            con.Close();
        }
         



        return datatable;
    }


}