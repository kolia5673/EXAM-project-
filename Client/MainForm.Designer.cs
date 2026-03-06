using System.Windows.Forms;

namespace Client
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtServer;
        private TextBox txtProducts;
        private Button btnSearch;
        private DataGridView grid;
        private Label lblTotal;
        private Label label1;
        private Label label2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtServer = new TextBox();
            this.txtProducts = new TextBox();
            this.btnSearch = new Button();
            this.grid = new DataGridView();
            this.lblTotal = new Label();
            this.label1 = new Label();
            this.label2 = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();

            label1.Text = "Адрес сервера";
            label1.Location = new System.Drawing.Point(20, 20);

            txtServer.Location = new System.Drawing.Point(150, 20);
            txtServer.Width = 300;
            txtServer.Text = "http://localhost:5000";

            label2.Text = "Товары (через запятую)";
            label2.Location = new System.Drawing.Point(20, 60);

            txtProducts.Location = new System.Drawing.Point(150, 60);
            txtProducts.Width = 300;

            btnSearch.Text = "Поиск";
            btnSearch.Location = new System.Drawing.Point(470, 60);
            btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            grid.Location = new System.Drawing.Point(20, 110);
            grid.Width = 550;
            grid.Height = 250;

            grid.Columns.Add("Name", "Название");
            grid.Columns.Add("Price", "Цена");
            grid.Columns.Add("Quantity", "Количество");
            grid.Columns.Add("Sum", "Сумма");

            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(20, 380);
            this.lblTotal.Size = new System.Drawing.Size(300, 20);
            this.lblTotal.Text = "Загальна сума:";
            this.lblTotal.Visible = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);

            this.ClientSize = new System.Drawing.Size(600, 420);
            this.Controls.Add(txtServer);
            this.Controls.Add(txtProducts);
            this.Controls.Add(btnSearch);
            this.Controls.Add(grid);
            this.Controls.Add(lblTotal);
            this.Controls.Add(label1);
            this.Controls.Add(label2);

            this.Text = "Product Client";

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}