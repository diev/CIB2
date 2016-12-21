﻿using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace NlgDBcredProg
{
    public partial class Form1 : Form

    {
        SqlConnection connection;
        DataSet dataSet;
        SqlDataAdapter adapterUsers, adapterFiles, adapterOOO, adapterLoan, adapterZalogodat, adapterPoruchit, adapterGroupObj, adapterFilesGrOb;
        BindingSource bindingSourceUsers, bindingSourceFiles, bindingSourceOOO, bindingSourceLoan, bindingSourceZalogodat, bindingSourcePoruchit, bindingSourceGroupObj, bindingSourceFilesGrOb;
        DataGridView gridUsers, gridFiles, gridOOO, gridLoan, gridZalogodat, gridPoruchit, gridGroupObj, gridFilesGrOb;

        public Form1()
        
        {
            InitializeComponent();
            Size = new Size(450, 450); 
            connection = new SqlConnection(@"Data Source=.\cibEXPRESS;Initial Catalog=usersdb;Integrated Security=True"); //connection to SQL
            adapterUsers = new SqlDataAdapter("SELECT * FROM Users;", connection); //select from tables+
            adapterOOO = new SqlDataAdapter("SELECT * FROM OOO", connection);
            adapterLoan = new SqlDataAdapter("SELECT * FROM LoanAgr", connection);
            adapterZalogodat = new SqlDataAdapter("SELECT * FROM Zalogodat", connection);
            adapterPoruchit = new SqlDataAdapter("SELECT * FROM Poruchit", connection);
            adapterFiles = new SqlDataAdapter("SELECT * FROM Files", connection);
            //adapterGroupObj = new SqlDataAdapter("SELECT * FROM GroupObj", connection);
            //adapterFilesGrOb = new SqlDataAdapter("SELECT * FROM FilesGrOb", connection);

            dataSet = new DataSet(); //create dataset with tables+
            adapterUsers.Fill(dataSet, "Users");
            adapterOOO.Fill(dataSet, "OOO");
            adapterLoan.Fill(dataSet, "LoanAgr");
            adapterZalogodat.Fill(dataSet, "Zalogodat");
            adapterPoruchit.Fill(dataSet, "Poruchit");
            adapterFiles.Fill(dataSet, "Files");
            //adapterGroupObj.Fill(dataSet, "GroupObj");
            //adapterFilesGrOb.Fill(dataSet, "FilesGrOb");

            dataSet.Relations.Add("ooo-loanagr", //relations OOO and LoanAgr      
            dataSet.Tables["OOO"].Columns["IdOOO"],             
            dataSet.Tables["LoanAgr"].Columns["LoanId"]);
            dataSet.Relations.Add("loanagr-users", //relations LoanAgr and Users (main data)  
            dataSet.Tables["LoanAgr"].Columns["Id"],
            dataSet.Tables["Users"].Columns["IdUsers"]);
            dataSet.Relations.Add("loanagr-zalogodat", //relations LoanAgr and Zalogodat
            dataSet.Tables["LoanAgr"].Columns["Id"],
            dataSet.Tables["Zalogodat"].Columns["ZalId"]);
            dataSet.Relations.Add("loanagr-poruchit", //relations LoanAgr and Poruchit
            dataSet.Tables["LoanAgr"].Columns["Id"],
            dataSet.Tables["Poruchit"].Columns["PorId"]);
            dataSet.Relations.Add("zalogodat-files", //relations Zalogod and Files      
            dataSet.Tables["Zalogodat"].Columns["FilesId"],             
            dataSet.Tables["Files"].Columns["ZalFilId"]);
            dataSet.Relations.Add("poruchit-files", //relations Poruchit and Files      
            dataSet.Tables["Poruchit"].Columns["FilesId"],
            dataSet.Tables["Files"].Columns["PorlFilId"]);

            bindingSourceOOO = new BindingSource(dataSet, "OOO"); //bs dataset+
            bindingSourceLoan = new BindingSource(dataSet, "LoanAgr");
            bindingSourceUsers = new BindingSource(dataSet, "Users");
            bindingSourceZalogodat = new BindingSource(dataSet, "Zalogodat");
            bindingSourcePoruchit = new BindingSource(dataSet, "Poruchit");
            bindingSourceFiles = new BindingSource(dataSet, "Files");
            //bindingSourceGroupObj = new BindingSource(dataSet, "GroupObj");
            //bindingSourceFilesGrOb = new BindingSource(dataSet, "FilesGrOb");

            bindingSourceLoan = new BindingSource(bindingSourceOOO, "ooo-loanagr"); //bs with relations
            bindingSourceZalogodat = new BindingSource(bindingSourceLoan, "loanagr-zalogodat");
            bindingSourcePoruchit = new BindingSource(bindingSourceLoan, "loanagr-poruchit");
            bindingSourceUsers = new BindingSource(bindingSourceLoan, "loanagr-users");
            bindingSourceFiles = new BindingSource(bindingSourceZalogodat, "zalogodat-files");
            bindingSourceFiles = new BindingSource(bindingSourcePoruchit, "poruchit-files");

            gridOOO = new DataGridView(); //dg OOO
            gridOOO.Size = new Size(300, 610);
            gridOOO.Location = new Point(5, 5);
            gridOOO.DataSource = bindingSourceOOO;
            gridLoan = new DataGridView(); //dg LoanAgr
            gridLoan.Size = new Size(250, 200);
            gridLoan.Location = new Point(310, 5);
            gridLoan.DataSource = bindingSourceLoan;
            gridUsers = new DataGridView(); //dg Users
            gridUsers.Size = new Size(350, 200);
            gridUsers.Location = new Point(570, 5);
            gridUsers.DataSource = bindingSourceUsers;
            gridZalogodat = new DataGridView(); //dg Zalogodat
            gridZalogodat.Size = new Size(300, 200);
            gridZalogodat.Location = new Point(310, 240);
            gridZalogodat.DataSource = bindingSourceZalogodat;
            gridPoruchit = new DataGridView(); //dg Poruchit
            gridPoruchit.Size = new Size(300, 200);
            gridPoruchit.Location = new Point(620, 240);
            gridPoruchit.DataSource = bindingSourcePoruchit;
            gridFiles = new DataGridView(); //dg Files
            gridFiles.Size = new Size(670, 200);
            gridFiles.Location = new Point(310, gridUsers.Bottom + 250); 
            gridFiles.DataSource = bindingSourceFiles;

            this.Controls.AddRange(new Control[] { gridUsers, gridFiles, gridOOO, gridLoan, gridZalogodat, gridPoruchit, gridGroupObj, gridFilesGrOb }); //control with 4 dg

            dataSet.Tables["OOO"].Columns["IdOOO"].ColumnMapping = MappingType.Hidden; // hidden ID's 
            dataSet.Tables["LoanAgr"].Columns["Id"].ColumnMapping = MappingType.Hidden;
            dataSet.Tables["LoanAgr"].Columns["LoanId"].ColumnMapping = MappingType.Hidden;
            dataSet.Tables["Users"].Columns["FILESId"].ColumnMapping = MappingType.Hidden;
            dataSet.Tables["Users"].Columns["IdUsers"].ColumnMapping = MappingType.Hidden;
        }

        private void saveButtonOOO_Click(object sender, EventArgs e) //save for OOO
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\cibEXPRESS;Initial Catalog=usersdb;Integrated Security=True"))
            {
                connection.Open();
                adapterOOO = new SqlDataAdapter("SELECT * FROM OOO;", connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapterOOO);
                adapterOOO.InsertCommand = new SqlCommand("sp_OOO", connection);
                adapterOOO.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapterOOO.InsertCommand.Parameters.Add(new SqlParameter("@наименование", SqlDbType.NVarChar, 50, "Наименование"));
                adapterOOO.InsertCommand.Parameters.Add(new SqlParameter("@принят", SqlDbType.Date, 30, "Принят"));
                SqlParameter parameter = adapterOOO.InsertCommand.Parameters.Add("@IdOOO", SqlDbType.Int, 10, "IdOOO");
                parameter.Direction = ParameterDirection.Output;
                adapterOOO.Update(dataSet.Tables["OOO"]);
            }
        }
        private void saveButtonLoan_Click(object sender, EventArgs e) //save for LoanAgr
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\cibEXPRESS;Initial Catalog=usersdb;Integrated Security=True"))
            {
                connection.Open();
                adapterLoan = new SqlDataAdapter("SELECT * FROM LoanAgr;", connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapterLoan);
                adapterLoan.InsertCommand = new SqlCommand("sp_Loan", connection);
                adapterLoan.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapterLoan.InsertCommand.Parameters.Add(new SqlParameter("@договор", SqlDbType.NVarChar, 50, "Договор"));
                adapterLoan.InsertCommand.Parameters.Add(new SqlParameter("@принят", SqlDbType.Date, 30, "Принят"));
                adapterLoan.InsertCommand.Parameters.Add(new SqlParameter("@loanid", SqlDbType.Int, 10, "LoanId"));
                SqlParameter parameter = adapterLoan.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 10, "Id");

                parameter.Direction = ParameterDirection.Output;
                adapterLoan.Update(dataSet.Tables["LoanAgr"]);
            }
        }
        private void saveButtonUsers_Click(object sender, EventArgs e) //save for Users
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\cibEXPRESS;Initial Catalog=usersdb;Integrated Security=True"))
            {
                connection.Open();
                adapterUsers = new SqlDataAdapter("SELECT * FROM Users;", connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapterUsers);
                adapterUsers.InsertCommand = new SqlCommand("sp_Users", connection);
                adapterUsers.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapterUsers.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NText, 10, "Name"));
                adapterUsers.InsertCommand.Parameters.Add(new SqlParameter("@age", SqlDbType.NText, 10, "Age"));
                adapterUsers.InsertCommand.Parameters.Add(new SqlParameter("@idusers", SqlDbType.Int, 10, "IdUsers"));
                SqlParameter parameter = adapterUsers.InsertCommand.Parameters.Add("@FILESId", SqlDbType.Int, 10, "FILESId");
                parameter.Direction = ParameterDirection.Output;
                adapterUsers.Update(dataSet.Tables["Users"]);
            }
        }
        private void saveButtonFiles_Click(object sender, EventArgs e) //save for Files in DB - TO-DO
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\cibEXPRESS;Initial Catalog=usersdb;Integrated Security=True"))
            {
                //some text to save information from Files (later)
            }
        }


        private void Form1_Load(object sender, EventArgs e) 
        {
            StartPosition = FormStartPosition.CenterScreen; //main form position and size
            this.Left += 400;
            Size = new Size(1000, 800); 
        }

        private void searchForm_Click(object sender, EventArgs e) //button to open Search form
        {
            Search src = new Search();
            src.Show();
        }
    }
}
