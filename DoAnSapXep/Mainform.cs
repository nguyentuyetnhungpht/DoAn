﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Resources;
using System.Globalization;
namespace DoAnSapXep
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
            //bo check loi crossthread
          Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// Các biến toàn cục
        /// </summary>
        public static bool isEnglish = false;
        CultureInfo culture;

        public int loaiThuatToan;
        HienThiThuatToan HienThuattoan = new HienThiThuatToan();
        private bool isRunning;
        private bool isTang;
        private int SoLuongNode;
        public List<int> DanhSachThamSo;
        public List<Node> DanhSachNode;
        public List<Label> danhSachLabel;
        public List<int> MangChuaSapXep;
        Random rank = new Random();
        private int Phut = 0;
        private int Giay = 0;


        Action ThuatToanSapXep;// giong delegate
        private Thread sapxepThread;
        //int a = 2;

        #region Biến i, j, k, pos,...

        private List<Label> labelSTTArr = new List<Label>();
        private Dictionary<string, Label> bienArr;
        #endregion


        #region Set Ngôn Ngữ
        void setLang(string cultureName)
        {
            culture = new CultureInfo(cultureName);
            ResourceManager rm = new ResourceManager("DoAnSapXep.Lang.myResource", typeof(Mainform).Assembly);
            //ManHinh_label.Text = rm.GetString("manhinh", culture);
            lbMangChuaSapXep.Text = rm.GetString("mangchuasapxep", culture);
            lbThuatToan.Text = rm.GetString("thuattoan", culture);
            lbSapXep.Text = rm.GetString("sapxep", culture);
            tangrdbtn.Text = rm.GetString("tang", culture);
            giamrdbtn.Text = rm.GetString("giam", culture);
            lbYTuongThuatToan.Text = rm.GetString("ytuongthuattoan", culture);
            lbDieuKhien.Text = rm.GetString("dieukhien", culture);
            lbTocDo.Text = rm.GetString("tocdo", culture);
            batdaubtn.Text = rm.GetString("batdau", culture);
            dungbtn.Text = rm.GetString("dung", culture);
            huybnt.Text = rm.GetString("huydieukhien", culture);
            lbKhoiTao.Text = rm.GetString("khoitao", culture);
            lbSoPhanTu.Text = rm.GetString("sophantu", culture);
            taongaunhienbtn.Text = rm.GetString("ngaunhien", culture);
            venutbtn.Text = rm.GetString("venut", culture);
            docfilebtn.Text = rm.GetString("docfile", culture);
            nhaptaybtn.Text = rm.GetString("nhapbangtay", culture);
            lbHuy.Text = rm.GetString("huyquatrinh", culture);
            xoamangbtn.Text = rm.GetString("xoamang", culture);
            thoatbtn.Text = rm.GetString("huyquatrinh", culture);
            lbNgonNgu.Text = rm.GetString("ngonngu", culture);
            this.Text = rm.GetString("fname", culture);
        }

        #region Set English
        private void btnEn_Click(object sender, EventArgs e)
        {
            isEnglish = true;
            HienThiThuatToan.yTuongThuatToan.Clear();
            addIdea();
            setLang("en-US");
        }
        #endregion

        #region Set Tiếng Việt

        private void btnVi_Click(object sender, EventArgs e)
        {
            isEnglish = false;
            HienThiThuatToan.yTuongThuatToan.Clear();
            addYTuong();
            setLang("vi-VN");

        }
        #endregion

        #endregion




        // Cac thong so mac dinh khi form duoc tao ra
        private void Mainform_Load(object sender, EventArgs e)
        {
            VeNut();
            KhoiTaoMacDinh();
            Mangchuasapxep();
            bienArr = new Dictionary<string, Label>();
            List<string> bienArrString = new List<string>() { "i", "j", "min", "right", "left", "k", "pos", "m", "vt_x", "gap", "a:", "b:", "c:" };
            foreach (string item in bienArrString)
            {
                bienArr.Add(item, new Label());
            }
            foreach (var item in bienArr)
            {

                this.sapxepPanel.Controls.Add(item.Value);
                item.Value.TextAlign = ContentAlignment.MiddleCenter;
            }

            bienArr["i"].Size = bienArr["j"].Size = new Size(ThamSo.KichCoNode, 15);
            
            bienArr["min"].Size = new Size(60, 20);
            bienArr["left"].Size = new Size(60, 20);
            bienArr["right"].Size = new Size(60, 20);
            bienArr["m"].Size = bienArr["k"].Size = new Size(40, 15);
            bienArr["pos"].Size = new Size(60, 20);
            bienArr["vt_x"].Size = new Size(60, 20);
            bienArr["gap"].Size = new Size(60, 20);
            bienArr["a:"].Size = bienArr["b:"].Size = bienArr["c:"].Size = new Size(40, 15);
            

            culture = CultureInfo.CurrentCulture;
            huybnt.Enabled = false;
            trackBar1.Maximum = ThamSo.ThoiGianDoi * 2;
            trackBar1.Minimum = 0;
            trackBar1.Value = ThamSo.ThoiGianDoi;
            trackBar1.LargeChange = 1;
        }
        private void KhoiTaoMacDinh()
        {
            interchangerdbtn.Checked = true;
            tangrdbtn.Checked = true;
            isRunning = false;
            soluongNodetbx.Text = "5";
            SoLuongNode = 5;
            VeNut();
            DieuChinhControls(isRunning);
            setMauAllControl();
        }

        private void setMauAllControl()
        {
            this.BackColor = Color.White;
            batdaubtn.BackColor = dungbtn.BackColor = huybnt.BackColor = taongaunhienbtn.BackColor
            = venutbtn.BackColor = docfilebtn.BackColor = nhaptaybtn.BackColor = xoamangbtn.BackColor = thoatbtn.BackColor = Color.White;
            DebugPanel.BackColor = thuattoanpanel.BackColor = Loaisapxeppanel.BackColor = panel10.BackColor = HienThiPanel.BackColor
            = dieukhienpanel.BackColor = khoitaopanel.BackColor = huypanel.BackColor = ngonngupanel.BackColor = Color.White;
            codeListBox.BackColor = chuasapxepPanel.BackColor =yTuongTextBox.BackColor= Color.White;
        }

        void ChonThuatToan()
        {
            if (interchangerdbtn.Checked == true)
            {
                ThuatToanSapXep = InterchangeSort;
            }
            if (bubblerdbtn.Checked == true)
            {
                ThuatToanSapXep = ThuatToanSapXep = BubbleSort;
            }
            if (selectionrdbtn.Checked == true)
            {
                ThuatToanSapXep = SelectionSort;
            }
            if (insertionrdbtn.Checked == true)
            {
                ThuatToanSapXep = InsertionSort;
            }
            if (shakerrdbtn.Checked == true)
            {
                ThuatToanSapXep = ShakerSort;
            }
            if (shellrdbtn.Checked == true)
            {
                ThuatToanSapXep = ShellSort;
            }
            if (heaprdbtn.Checked == true)
            {
                ThuatToanSapXep = HeapSort;
            }
            if (quickrdbtn.Checked == true)
            {
                ThuatToanSapXep = QuickSort;
            }
            if (binaryinsertionrdbtn.Checked == true)
            {
                ThuatToanSapXep = BinaryInsertionSort;
            }
            if (mergerdbtn.Checked == true)
            {
                ThuatToanSapXep = MergeSort;
            }
        }

        private void DieuChinhControls(bool isRunning)
        {
            if (isRunning == true)
            {
                daydangxepListbox.Items.Clear();
                daydangxepListbox.BringToFront();
                thuattoanpanel.Enabled = khoitaopanel.Enabled = ngonngupanel.Enabled = batdaubtn.Enabled = Loaisapxeppanel.Enabled = HienThiPanel.Enabled = false;
            }
            else
            {
               
                thuattoanpanel.Enabled = khoitaopanel.Enabled = ngonngupanel.Enabled = batdaubtn.Enabled = Loaisapxeppanel.Enabled = HienThiPanel.Enabled = true;

            }
        }

        /// <summary>
        /// Khi sự kiện text đã thay đổi kiểm tra xem slnodetbx có là rỗng hay không nếu rổng rán lại số lượng node=0 vì trường hợp người dùng
        /// đã nhập 1 giá trị ví dụ số 12 mà muốn đổi thành số 20, nếu không có điều kiện sẽ bị lỗi không thể chuyển ra kiểu int được
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

            if (soluongNodetbx.Text == "")
            {
                SoLuongNode = 0;
            }
            else
            {
                SoLuongNode = Int32.Parse(soluongNodetbx.Text);
            }
        }
        /// <summary>
        /// Bắt sự kiện keyPress chỉ cho phép nhập số vào tbx
        /// Char.IsDigit(e.KeyChar) kiểm tra xem phím vừa nhập vào textbox có phải là ký tự số hay không, hàm này trả về kiểu bool
        /// Char.IsContro(e.KeyChar) –> kiểm tra xem phím vừa nhập vào textbox có phải là các ký tự điều khiển(nếu k có nó sẽ bắt nút enter,delete... như là không phải là số)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;// dừng tất cả các tiến trình có liên quan đến thằng e

            }
        }

        private void taongaunhienbtn_Click(object sender, EventArgs e)
        {

            SoLuongNode = rank.Next(2, ThamSo.SoluongNodeLonNhat);
            VeNut();
            soluongNodetbx.Text = SoLuongNode.ToString();
           
       
        }

        private void HienThiPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void venutbtn_Click(object sender, EventArgs e)
        {
            if (SoLuongNode < 2 || SoLuongNode > ThamSo.SoluongNodeLonNhat)
            {
                MessageBox.Show("Số lượng Node phải lớn hơn hoặc bằng 2 và nhỏ hơn " + ThamSo.SoluongNodeLonNhat);
                soluongNodetbx.Focus();
                return;
            }
            VeNut();
            Mangchuasapxep();
        }
        /// <summary>
        /// Khởi tạo 2 list 1 list int lưu giá trị dãy 1 list btn hiện thị trên GUI
        /// </summary>
        void VeNut()
        {
            DanhSachThamSo = new List<int>(SoLuongNode);
            DanhSachNode = new List<Node>(SoLuongNode);
            danhSachLabel = new List<Label>(SoLuongNode);

            sapxepPanel.Controls.Clear(); // Xóa những btn cũ trên panel ở phiên làm việc
            int temp = -SoLuongNode / 2;
            int temp2 = ThamSo.KhoangCachCacNode / 2;
            int temp3 = temp + 1;
            Point newPoint;
            for (int i = 0; i < SoLuongNode; i++)
            {
                Label tam = new Label() { Text = i.ToString() };
                tam.Size = new Size(ThamSo.KichCoNode / 2, ThamSo.KichCoNode / 2);
                tam.ForeColor = Color.Blue;
                //tam.Enabled = false;
                Node btn = new Node(i);
                int Value = rank.Next(2, 100);
                btn.Value = Value;
                btn.Text = Value.ToString();
                btn.BackColor = ThamSo.mauNen;
                if (SoLuongNode % 2 != 0)
                {
                    newPoint = new Point(sapxepPanel.Width / 2 - btn.Width / 2,
                                         sapxepPanel.Height / 2 - btn.Height / 2);
                    btn.Location = new Point(newPoint.X + temp * btn.Width + temp * ThamSo.KhoangCachCacNode, newPoint.Y);
                    temp++;
                    tam.Location = new Point(btn.Location.X+btn.Width/2 - tam.Width / 2, btn.Location.Y + 120);
                }
                else
                {
                    if (i == SoLuongNode / 2)
                    {
                        temp2 = -temp2;
                        temp3 = 0;
                    }

                    newPoint = new Point(sapxepPanel.Width / 2 - btn.Width / 2 + ThamSo.KichCoNode / 2, sapxepPanel.Height / 2 - btn.Height / 2);
                    btn.Location = new Point(newPoint.X - temp2 + temp3 * ThamSo.KhoangCachCacNode + temp * btn.Width, newPoint.Y);// Node Xuất hiện giữa panel
                    tam.Location = new Point(btn.Location.X + btn.Width / 2-tam.Width/2, btn.Location.Y + 120);
                    temp++;
                    temp3++;
                }
                sapxepPanel.Controls.Add(tam);
                sapxepPanel.Controls.Add(btn);
                DanhSachNode.Add(btn);
                DanhSachThamSo.Add(Value);
                danhSachLabel.Add(tam);
                btn.Capnhat += Btn_Capnhat;
            }
            if (isRunning == false)
            {
                MangChuaSapXep = new List<int>(SoLuongNode);
                for (int i = 0; i < SoLuongNode; i++)
                {
                    MangChuaSapXep.Add(DanhSachThamSo[i]);
                }
            }
            Mangchuasapxep();
            batdaubtn.Enabled = true;

        }

        private void Mangchuasapxep()
        {
            chuasapxepPanel.Controls.Clear();
            string _string = "";
            foreach (var item in DanhSachThamSo)
            {
                _string += item + "    ";
            }
            Label lbtext = new Label() { Text = _string };
            lbtext.Size = new Size(6000, 20);
            lbtext.Location = new Point(30, 12);
            chuasapxepPanel.Controls.Add(lbtext);
        }

        private void Hienthimangdangsapxep(int k, int j, string chuDau, string chuCuoi)
        {
            string temp = "\n" + chuDau + "=" + k + ", " + chuCuoi + "=" + j + " : ";
            for (int i = 0; i < SoLuongNode; i++)
            {
                int t = DanhSachThamSo[i];
                temp += t.ToString() + "  ";
            }
       
            daydangxepListbox.Items.Add(temp);
        }

        private void Btn_Capnhat(object sender, EventArgs e)
        {
            for (int i = 0; i < SoLuongNode; i++)
            {
                DanhSachThamSo[i] = DanhSachNode[i].Value;
            }
            Mangchuasapxep();
        }

        private void radiobtn_CheckedChanged(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                hienThiThuatToanNCodeC();
            }
            isTang = true;
            daydangxepListbox.Visible = false;
        }
        #region Thêm ý tưởng vào yTuongTextBox
        private void addYTuong()
        {
            this.yTuongTextBox.Controls.Clear();
            this.yTuongTextBox.Text = HienThiThuatToan.yTuongVi;
        }
        private void addIdea()
        {
            this.yTuongTextBox.Controls.Clear();
            this.yTuongTextBox.Text = HienThiThuatToan.yTuongEn;
        }
        #endregion

        #region Hiển thị code c ++ và ý tưởng thuật toán
        private void hienThiThuatToanNCodeC()
        {
            if (isEnglish == true)
            {
                addIdea();
            }
            else
            {
                addYTuong();
            }
            //Gán yTuongTextBox, codeListBox và isSapXepTang
            HienThiThuatToan.yTuongThuatToan = yTuongTextBox;
            HienThiThuatToan.codeListBox = codeListBox;
            bool tang = tangrdbtn.Checked;
            //chọn sort
            if (selectionrdbtn.Checked)
            {
                HienThiThuatToan.SelectionSort(tang);

            }
            else if (insertionrdbtn.Checked)
            {
                HienThiThuatToan.InsertionSort(tang);
            }
            else if (shakerrdbtn.Checked)
            {
                HienThiThuatToan.ShakerSort(tang);
            }
            else if (quickrdbtn.Checked)
            {
                HienThiThuatToan.QuickSort(tang);
            }
            else if (shellrdbtn.Checked)
            {
                HienThiThuatToan.ShellSort(tang);
            }
            else if (interchangerdbtn.Checked)
            {
                HienThiThuatToan.InterchangeSort(tang);
            }
            else if (binaryinsertionrdbtn.Checked)
            {
                HienThiThuatToan.BinaryInsertionSort(tang);
            }
            else if (bubblerdbtn.Checked)
            {
                HienThiThuatToan.BubbleSort(tang);
            }
            else if (heaprdbtn.Checked)
            {
                HienThiThuatToan.HeapSort(tang);
            }
            else if (mergerdbtn.Checked)
            {
                HienThiThuatToan.MergeSort(tang);
            }

        }
        #endregion


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DemThoiGian();
        }
        private void DemThoiGian()
        {
            Giay++;
            if (Giay > 59)
            {
                Giay = 0;
                Phut++;
            }
           
            if (Giay<10)
            {
                label11.Text = Phut + ":0" + Giay;
            }
            else
            {
                label11.Text = Phut + ":" + Giay;
            }
            
        }


        void batdaubtn_Click(object sender, EventArgs e)
        {
            daydangxepListbox.Visible = true;
            huybnt.Enabled = true;
            isRunning = true;
            DieuChinhControls(isRunning);
            Reset_CountTime();
            timer1.Start();
            ChonThuatToan();
            sapxepThread = new Thread(new ThreadStart(ThuatToanSapXep));
            sapxepThread.Start();
           
            //backgroundWorker1.RunWorkerAsync(); // goi ham do work  

        }
        #region Tạm dừng
        // Tạm dừng
        public static ManualResetEvent codeListBoxPauseStatus = new ManualResetEvent(true);
        public static bool CodeListBoxIsPause = false;


        private void dungbtn_Click(object sender, EventArgs e)
        {
            if (Node.IsPause)
            {
                Node.pauseStatus.Set();     // hàm để resume
                Node.IsPause = false;
                dungbtn.Text = "Tạm dừng";
                timer1.Start();
            }
            else
            {
                Node.pauseStatus.Reset();    // hàm để pause
                Node.IsPause = true;
                dungbtn.Text = "Tiếp tục";
                timer1.Stop();
            }
            //if (CodeListBoxIsPause)
            //{
            //    codeListBoxPauseStatus.Set();
            //    CodeListBoxIsPause = false;
            //    dungbtn.Text = "Tạm dừng";
            //    timer1.Start();
            //}
            //else
            //{
            //    codeListBoxPauseStatus.Reset();
            //    CodeListBoxIsPause = true;
            //    dungbtn.Text = "Tiếp tục";
            //    timer1.Stop();
            //}

        }
        #endregion

        /// <summary>
        /// Khu vuc bao tri 
        /// dang bi loi
        /// </summary>
        #region thu dung cach co san

        public void DiChuyenNodeDen(object oNode, object oVitriMoi)
        {
            int vitriMoi = (int)oVitriMoi;
            Node node = oNode as Node;
            if (vitriMoi > node.vitriHienTai)
            {
                node.ChuyenLen();
                node.ChuyenNgang(vitriMoi);
                node.ChuyenXuong();
            }
            else if (vitriMoi < node.vitriHienTai)
            {
                node.ChuyenXuong();
                node.ChuyenNgang(vitriMoi);
                node.ChuyenLen();
            }

      // set lai vi tri moi
            node.vitriHienTai = vitriMoi;
        }

        // Hoán vị chổ của 2 node
        Task hoanVi1Task;
        Task hoanVi2Task;
        private void DichuyenCacNode(int vitriNodeA, int vitriNodeB)
        {
            DanhSachNode[vitriNodeA].BackColor = DanhSachNode[vitriNodeB].BackColor = ThamSo.mauNodeDangSX;
            // Cách dùng task
            hoanVi1Task = Task.Factory.StartNew(() => { DiChuyenNodeDen(DanhSachNode[vitriNodeA], vitriNodeB); });
            hoanVi2Task = Task.Factory.StartNew(() => { DiChuyenNodeDen(DanhSachNode[vitriNodeB], vitriNodeA); });
            // note: Task.Factory.StartNew = THÊM task vào cuối hàng đợi và CHẠY ngay khi có thể

            Task.WaitAll(hoanVi1Task, hoanVi1Task);

            // Thay đổi vị trí của node trong mảng nodeArray
            if (DanhSachNode.Count != 0)                   //check xem nếu mảng còn tồn tại --> trong trường hợp mảng đã bị hủy
            {
                CapNhatDanhSachNode(vitriNodeA, vitriNodeB);
            }

            // Đổi màu 2 node sau khi sắp xếp
            DanhSachNode[vitriNodeA].BackColor = DanhSachNode[vitriNodeB].BackColor = ThamSo.mauNodeHTSX;
        }



        #endregion

        #region Cac Ham cap nhat lai mang va Node
        private void CapNhatThamSo(int vt1, int vt2)
        {
            int temp = DanhSachThamSo[vt1];
            DanhSachThamSo[vt1] = DanhSachThamSo[vt2];
            DanhSachThamSo[vt2] = temp;
            DanhSachNode[vt1].BackColor = DanhSachNode[vt2].BackColor = ThamSo.mauNodeDangSX;



        }
        private void CapNhatDanhSachNode(int vt1, int vt2)
        {
            
            Node temp = DanhSachNode[vt1];
            DanhSachNode[vt1] = DanhSachNode[vt2];
            DanhSachNode[vt2] = temp;
            DanhSachNode[vt1].BackColor = DanhSachNode[vt2].BackColor = ThamSo.mauNodeDangSX;
        }
        #endregion

        //#region Khung chuong trinh
        ///// <summary>
        ///// Su dung backgroundWalker de mo phong su di chuyen cua cac Node
        ///// </summary>
        /////  Vi tri cua Node Dau tien<param name="vt1"></param>
        ///// Vi tri Node tiep theo<param name="vt2"></param>
        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //Do work xu ly du lieu nhung khong the cap nhat len Gui
        //    // Ham dowork thuc hien xong goi ham complete
        //    //    while (isRunning==true)
        //    //    {

        //    //backgroundWorker1.ReportProgress(0);
        //    //Thread.Sleep(1000);
        //    //    }
        //    //e.Cancel = true;
        //    HienThiThuatToan.ChayCodeC(1);
        //    if (interchangerdbtn.Checked == true)
        //    {
        //        InterchangeSort();
        //    }
        //    if (bubblerdbtn.Checked == true)
        //    {
        //        BubbleSort();
        //    }
        //    if (selectionrdbtn.Checked == true)
        //    {
        //        SelectionSort();
        //    }
        //    if (insertionrdbtn.Checked == true)
        //    {
        //        InsertionSort();
        //    }
        //    if (shakerrdbtn.Checked == true)
        //    {
        //        ShakerSort();
        //    }
        //    if (shellrdbtn.Checked == true)
        //    {
        //        ShellSort();
        //    }
        //    if (heaprdbtn.Checked == true)
        //    {
        //        HeapSort();
        //    }
        //    if (quickrdbtn.Checked == true)
        //    {
        //        QuickSort();
        //    }
        //    if (binaryinsertionrdbtn.Checked==true)
        //    {
        //        BinaryInsertionSort();
        //    }
        //    if (mergerdbtn.Checked==true)
        //    {
        //        MergeSort();
        //    }

        //}
        //private void DichuyenCacNode(int vt1, int vt2)
        //{

        //    int Height = ThamSo.DoCaoDiChuyen + 10;
        //    int _width = Math.Abs(vt1 - vt2) * (ThamSo.KichCoNode + ThamSo.KhoangCachCacNode);
        //    status st = new status();
        //    st.vt1 = vt1;
        //    st.vt2 = vt2;

        //    st.Type = LoaiDiChuyen.Di_LEN_DI_XUONG;
        //    for (int i = 0; i < Height; i = i + 1)
        //    {
        //        // goiham progresschanged de cap nhat lai giao dien
        //        // Khi tao brackgroundworker nho set reportprogree la true k se bi 

        //        backgroundWorker1.ReportProgress(0, st);
        //        Thread.Sleep(ThamSo.ThoiGianDoi);
        //    }
        //    st.Type = LoaiDiChuyen.QUA_PHAI_QUA_TRAI;

        //    for (int i = 0; i < _width; i = i + 2)
        //    {


        //        backgroundWorker1.ReportProgress(0, st);
        //        Thread.Sleep(ThamSo.ThoiGianDoi);
        //    }
        //    st.Type = LoaiDiChuyen.DI_XUONG_DI_LEN;
        //    for (int i = 0; i < Height; i = i + 1)
        //    {

        //        backgroundWorker1.ReportProgress(0, st);
        //        Thread.Sleep(ThamSo.ThoiGianDoi);
        //    }
        //    st.Type = LoaiDiChuyen.DUNG;
        //    backgroundWorker1.ReportProgress(0, st);
        //    Thread.Sleep(ThamSo.ThoiGianDoi);
        //}


        //private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    /// Cap nhat Gui sau do goi lai ham dowork
        //    status st = e.UserState as status;// lay gia tri st truyen vao ham trn
        //    if (st == null)
        //    {
        //        return;
        //    }
        //    if (st.Type == LoaiDiChuyen.DUNG)
        //    {
        //        DanhSachNode[st.vt1].BackColor = DanhSachNode[st.vt2].BackColor = Color.Honeydew;
        //        CapNhatDanhSachNode(st.vt2, st.vt1);

        //        return;

        //    }

        //    if (st.Type == LoaiDiChuyen.Di_LEN_DI_XUONG)
        //    {
        //        DanhSachNode[st.vt1].Location = new Point(DanhSachNode[st.vt1].Location.X, DanhSachNode[st.vt1].Location.Y + 1);
        //        DanhSachNode[st.vt2].Location = new Point(DanhSachNode[st.vt2].Location.X, DanhSachNode[st.vt2].Location.Y - 1);
        //    }

        //    if (st.Type == LoaiDiChuyen.QUA_PHAI_QUA_TRAI)
        //    {
        //        DanhSachNode[st.vt1].Location = new Point(DanhSachNode[st.vt1].Location.X - 2, DanhSachNode[st.vt1].Location.Y);
        //        DanhSachNode[st.vt2].Location = new Point(DanhSachNode[st.vt2].Location.X + 2, DanhSachNode[st.vt2].Location.Y);
        //    }

        //    if (st.Type == LoaiDiChuyen.DI_XUONG_DI_LEN)
        //    {
        //        DanhSachNode[st.vt1].Location = new Point(DanhSachNode[st.vt1].Location.X, DanhSachNode[st.vt1].Location.Y - 1);
        //        DanhSachNode[st.vt2].Location = new Point(DanhSachNode[st.vt2].Location.X, DanhSachNode[st.vt2].Location.Y + 1);
        //    }


        //}

        //private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ///xong
        //    timer1.Stop();


        //    MessageBox.Show("Sắp xếp hoàn tất");
        //    isRunning = false;
        //    DieuChinhControls(isRunning);

        //}
        //#endregion
        private void Reset_CountTime()
        {

            Phut = Giay = 0;
            label11.Text = "00:00";
        }
        private void hoanTatSapXep()
        {
            HienThiThuatToan.ChayCodeC(1);
            isRunning = false;
            DieuChinhControls(isRunning);
            MessageBox.Show("Hoàn tất sắp xếp!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            timer1.Stop();

            sapxepThread.Abort();
            foreach (Label label in bienArr.Values)
            {
                label.Visible = false;
            }
            if (!isRunning)
            {
                timer1.Stop();
            }
            huybnt.Enabled = false;
        }
        #region InterchangeSort
        private void InterchangeSort()
        {

            int i = 0, j = 0;
            bool flag;

            HienThiThuatToan.ChayCodeC(2);
            for (i = 0; i < SoLuongNode - 1; i++)
            {
                bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                bienArr["i"].Text = "i = " + i;   
                sapxepPanel.Controls.Add(bienArr["i"]);
                bienArr["i"].Visible = true;
                HienThiThuatToan.ChayCodeC(3);
                for (j = i + 1; j < SoLuongNode; j++)
                {
                    bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                    bienArr["j"].Text = "j = " + j;
                    sapxepPanel.Controls.Add(bienArr["j"]);
                    bienArr["j"].Visible = true;
                    HienThiThuatToan.ChayCodeC(4);
                                       
                    flag = false;
                    if (tangrdbtn.Checked == true)
                    {
                        HienThiThuatToan.ChayCodeC(5);
                        if (DanhSachThamSo[j] < DanhSachThamSo[i]) flag = true;
                    }
                    if (giamrdbtn.Checked == true)
                    {
                        HienThiThuatToan.ChayCodeC(5);
                        if (DanhSachThamSo[j] > DanhSachThamSo[i]) flag = true;
                    }
                    if (flag)
                    {
                        HienThiThuatToan.ChayCodeC(6);
                        Thread.Sleep(ThamSo.ThoiGianDoi);
                        
                        CapNhatThamSo(i, j);
                        DichuyenCacNode(j, i);

                        Hienthimangdangsapxep(i, j, "i", "j");
                    }
                    

                }

            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();

        }


        #endregion
        #region SelectionSort
        private void SelectionSort()
        {

            int min, i = 0, j = 0;
            bool flag = false;


            HienThiThuatToan.ChayCodeC(2);
            for (i = 0; i < SoLuongNode - 1; i++)
            {

                HienThiThuatToan.ChayCodeC(3);
                HienThiThuatToan.ChayCodeC(5);
                bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                bienArr["i"].Text = "i = " + i;
                sapxepPanel.Controls.Add(bienArr["i"]);
                bienArr["i"].Visible = true;
                for (j = i + 1; j < SoLuongNode; j++)
                {
                    HienThiThuatToan.ChayCodeC(6);
                    flag = false;
                    min = i;
                    bienArr["min"].Location = new Point(danhSachLabel[min].Location.X, danhSachLabel[min].Location.Y - 140);
                    bienArr["min"].Text = "min = " + min;
                    bienArr["min"].SendToBack();
                    sapxepPanel.Controls.Add(bienArr["min"]);
                    bienArr["min"].Visible = true;
                    if (tangrdbtn.Checked == true)
                    {

                        HienThiThuatToan.ChayCodeC(7);
                        bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                        bienArr["j"].Text = "j = " + j;
                        sapxepPanel.Controls.Add(bienArr["j"]);
                        bienArr["j"].Visible = true;
                        if (DanhSachThamSo[j] < DanhSachThamSo[min])
                        {

                            min = j;

                            flag = true;
                            bienArr["min"].Location = new Point(danhSachLabel[min].Location.X, danhSachLabel[min].Location.Y - 140);
                            bienArr["min"].Text = "min = " + min;
                            bienArr["min"].SendToBack();
                        }
                    }
                    if (giamrdbtn.Checked == true)
                    {

                        HienThiThuatToan.ChayCodeC(7);
                        bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                        bienArr["j"].Text = "j = " + j;
                        sapxepPanel.Controls.Add(bienArr["j"]);
                        if (DanhSachThamSo[j] > DanhSachThamSo[min])
                        {

                            min = j;

                            flag = true;
                            bienArr["min"].Location = new Point(danhSachLabel[min].Location.X, danhSachLabel[min].Location.Y - 140);
                            bienArr["min"].Text = "min = " + min;
                            bienArr["min"].SendToBack();

                        }
                    }
                    if (flag == true)
                    {
                        HienThiThuatToan.ChayCodeC(8);
                        HienThiThuatToan.ChayCodeC(9);
                        Thread.Sleep(ThamSo.ThoiGianDoi);
                        DanhSachNode[i].BackColor = DanhSachNode[min].BackColor = Color.Green;
                        CapNhatThamSo(min, i);
                        DichuyenCacNode(min, i);
                        Hienthimangdangsapxep(i, j, "i", "j");
                    }
                }
            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();
        }
        #endregion
        #region BubbleSort
        private void BubbleSort()
        {
            int i, j;
            bool flag = false;
            HienThiThuatToan.ChayCodeC(2);
            HienThiThuatToan.ChayCodeC(3);
            for (i = 0; i < SoLuongNode - 1; i++)
            {
                HienThiThuatToan.ChayCodeC(4);
                bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                bienArr["i"].Text = "i = " + i;
                sapxepPanel.Controls.Add(bienArr["i"]);
                bienArr["i"].Visible = true;
                for (j = SoLuongNode - 1; j > i; j--)
                {
                    flag = false;
                    if (tangrdbtn.Checked == true)
                    {
                        HienThiThuatToan.ChayCodeC(5);
                        if (DanhSachThamSo[j] < DanhSachThamSo[j - 1]) flag = true;
                        bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                        bienArr["j"].Text = "j = " + j;
                        sapxepPanel.Controls.Add(bienArr["j"]);
                        bienArr["j"].Visible = true;
                    }
                    if (giamrdbtn.Checked == true)
                    {
                        HienThiThuatToan.ChayCodeC(5);
                        if (DanhSachThamSo[j] > DanhSachThamSo[j - 1]) flag = true;
                        bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                        bienArr["j"].Text = "j = " + j;
                        sapxepPanel.Controls.Add(bienArr["j"]);
                        bienArr["j"].Visible = true;
                    }
                    if (flag == true)
                    {
                        HienThiThuatToan.ChayCodeC(6);
                        Thread.Sleep(ThamSo.ThoiGianDoi);
                        DanhSachNode[j].BackColor = DanhSachNode[j - 1].BackColor = Color.Green;
                        CapNhatThamSo(j, j - 1);
                        DichuyenCacNode(j, j - 1);
                        Hienthimangdangsapxep(i, j, "i", "j");
                    }

                }
            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();

        } //xong
        #endregion
        #region InsertionSort
        private void InsertionSort()
        {
            int pos, key;

            Node Nodetam;
            DanhSachNode[0].BackColor = ThamSo.mauNodeDangSX;
            for (int i = 1; i < SoLuongNode; i++)
            {
                HienThiThuatToan.ChayCodeC(5);

                DanhSachNode[i].BackColor = ThamSo.mauNodeDangSX;

                key = DanhSachThamSo[i];
                HienThiThuatToan.ChayCodeC(7);
                Nodetam = DanhSachNode[i];
                pos = i - 1;

                bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                bienArr["i"].Text = "i = " + i;
                sapxepPanel.Controls.Add(bienArr["i"]);
                bienArr["i"].Visible = true;

                DanhSachNode[i].ChuyenLen();

                DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;

                HienThiThuatToan.ChayCodeC(8);

                bienArr["pos"].Location = new Point(danhSachLabel[pos].Location.X, danhSachLabel[pos].Location.Y - 200);
                bienArr["pos"].Text = "pos = " + pos;
                sapxepPanel.Controls.Add(bienArr["pos"]);
                bienArr["pos"].Visible = true;

                if (tangrdbtn.Checked == true)
                {
                    while ((pos >= 0) && (DanhSachThamSo[pos] > key))
                    {
                        DanhSachNode[pos].ChuyenNgang(pos + 1);
                        DanhSachNode[pos].vitriHienTai = pos + 1;
                        DanhSachNode[pos + 1] = DanhSachNode[pos];
                        DanhSachThamSo[pos + 1] = DanhSachThamSo[pos];

                        Hienthimangdangsapxep(pos, pos + 1, "pos", "i");

                        HienThiThuatToan.ChayCodeC(10);

                        pos--;
                    }

                }
                else
                {
                    while ((pos >= 0) && (DanhSachThamSo[pos] < key))
                    {
                        DanhSachNode[pos].ChuyenNgang(pos + 1);
                        DanhSachNode[pos].vitriHienTai = pos + 1;
                        DanhSachNode[pos + 1] = DanhSachNode[pos];
                        DanhSachThamSo[pos + 1] = DanhSachThamSo[pos];
                        Hienthimangdangsapxep(pos, pos + 1, "pos", "i");
                        HienThiThuatToan.ChayCodeC(10);
                        pos--;
                    }
                }
                Nodetam.ChuyenNgang(pos + 1);
                Nodetam.ChuyenXuong();

                DanhSachThamSo[pos + 1] = key;
                DanhSachNode[pos + 1] = Nodetam;
                Nodetam.vitriHienTai = pos + 1;

                DanhSachNode[pos + 1].BackColor = ThamSo.mauNodeHTSX;

            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();
        }
        #endregion
        #region ShakerSort
        private void ShakerSort()
        {

            HienThiThuatToan.ChayCodeC(2);
            int j, left, right, k;

            HienThiThuatToan.ChayCodeC(3);
            left = 0; right = DanhSachThamSo.Count - 1; k = DanhSachThamSo.Count - 1;

            HienThiThuatToan.ChayCodeC(4);
            bienArr["left"].Location = new Point(danhSachLabel[left].Location.X, danhSachLabel[left].Location.Y - 140);
            bienArr["left"].Text = "left = " + left;
            sapxepPanel.Controls.Add(bienArr["left"]);
            bienArr["left"].Visible = true;
            bienArr["right"].Location = new Point(danhSachLabel[right].Location.X, danhSachLabel[right].Location.Y - 160);
            bienArr["right"].Text = "right = " + right;
            sapxepPanel.Controls.Add(bienArr["right"]);
            bienArr["right"].Visible = true;
            bienArr["k"].Location = new Point(danhSachLabel[k].Location.X, danhSachLabel[k].Location.Y - 60);
            bienArr["k"].Text = "k = " + k;
            sapxepPanel.Controls.Add(bienArr["k"]);
            bienArr["k"].Visible = true;

            while (left < right)
            {

                HienThiThuatToan.ChayCodeC(6);

                j = right;
                bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                bienArr["j"].Text = "j = " + j;
                sapxepPanel.Controls.Add(bienArr["j"]);
                bienArr["j"].Visible = true;
                bienArr["j"].SendToBack();

                for (j = right; j > left; j--)
                {

                    bool thucHien = false;
                    if (tangrdbtn.Checked == true)
                    {
                        if (DanhSachThamSo[j] < DanhSachThamSo[j - 1])
                        {
                            thucHien = true;
                        }
                    }
                    else
                    {
                        if (DanhSachThamSo[j] > DanhSachThamSo[j - 1])
                        {
                            thucHien = true;
                        }
                    }

                    HienThiThuatToan.ChayCodeC(7);
                    if (thucHien)
                    {
                        HienThiThuatToan.ChayCodeC(9);
                        CapNhatThamSo(j, j - 1);
                        DichuyenCacNode(j, j - 1);
                        Hienthimangdangsapxep(j - 1, j, "k", "j"); // mang dang sap xep

                        HienThiThuatToan.ChayCodeC(10);
                        k = j;
                        bienArr["k"].Location = new Point(danhSachLabel[k].Location.X, danhSachLabel[k].Location.Y - 60);
                        bienArr["k"].Text = "k = " + k;
                        sapxepPanel.Controls.Add(bienArr["k"]);
                    }

                    HienThiThuatToan.ChayCodeC(6);
                    bienArr["k"].Location = new Point(danhSachLabel[k].Location.X, danhSachLabel[k].Location.Y - 60);
                    bienArr["k"].Text = "k = " + k;
                    sapxepPanel.Controls.Add(bienArr["k"]);

                }

                HienThiThuatToan.ChayCodeC(12);
                left = k;
                bienArr["left"].Location = new Point(danhSachLabel[left].Location.X, danhSachLabel[left].Location.Y - 140);
                bienArr["left"].Text = "left = " + left;
                sapxepPanel.Controls.Add(bienArr["left"]);


                HienThiThuatToan.ChayCodeC(13);
                j = left;
                bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                bienArr["j"].Text = "j = " + j;
                sapxepPanel.Controls.Add(bienArr["j"]);

                for (j = left; j < right; j++)
                {

                    bool thucHien = false;
                    if (tangrdbtn.Checked == true)
                    {
                        if (DanhSachThamSo[j + 1] < DanhSachThamSo[j])
                            thucHien = true;
                    }
                    else
                    {
                        if (DanhSachThamSo[j + 1] > DanhSachThamSo[j])
                            thucHien = true;
                    }

                    HienThiThuatToan.ChayCodeC(14);
                    if (thucHien)
                    {
                        HienThiThuatToan.ChayCodeC(16);
                        CapNhatThamSo(j, j + 1);
                        DichuyenCacNode(j + 1, j);
                        Hienthimangdangsapxep(j, j + 1, "k", "j");

                        HienThiThuatToan.ChayCodeC(17);
                        k = j;
                        bienArr["k"].Location = new Point(danhSachLabel[k].Location.X, danhSachLabel[k].Location.Y - 60);
                        bienArr["k"].Text = "k = " + k;
                        sapxepPanel.Controls.Add(bienArr["k"]);
                    }


                    HienThiThuatToan.ChayCodeC(13);
                    bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                    bienArr["j"].Text = "j = " + j;
                    sapxepPanel.Controls.Add(bienArr["j"]);

                }

                HienThiThuatToan.ChayCodeC(19);
                right = k;
                bienArr["right"].Location = new Point(danhSachLabel[right].Location.X, danhSachLabel[right].Location.Y - 160);
                bienArr["right"].Text = "right = " + right;
                sapxepPanel.Controls.Add(bienArr["right"]);

                HienThiThuatToan.ChayCodeC(4);
                bienArr["left"].Location = new Point(danhSachLabel[left].Location.X, danhSachLabel[left].Location.Y - 140);
                bienArr["left"].Text = "left = " + left;
                sapxepPanel.Controls.Add(bienArr["left"]);
                bienArr["right"].Location = new Point(danhSachLabel[right].Location.X, danhSachLabel[right].Location.Y - 160);
                bienArr["right"].Text = "right = " + right;
                sapxepPanel.Controls.Add(bienArr["right"]);



            }

            HienThiThuatToan.ChayCodeC(4);
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();
        }
        #endregion
        #region ShellSort
        private void ShellSort()
        {
            HienThiThuatToan.ChayCodeC(1);
            for (int gap = SoLuongNode / 2; gap > 0; gap /= 2)
            {
                HienThiThuatToan.ChayCodeC(3);
                bienArr["gap"].Location = new Point(danhSachLabel[gap].Location.X, danhSachLabel[gap].Location.Y - 180);
                bienArr["gap"].Text = "gap = " + gap;
                sapxepPanel.Controls.Add(bienArr["gap"]);
                bienArr["gap"].SendToBack();
                bienArr["gap"].Visible = true;
                for (int i = gap; i < SoLuongNode; i++)
                {
                    bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                    bienArr["i"].Text = "i = " + i;
                    sapxepPanel.Controls.Add(bienArr["i"]);
                    bienArr["i"].Visible = true;
                    HienThiThuatToan.ChayCodeC(5);
                    if (tangrdbtn.Checked == true)
                    {
                        HienThiThuatToan.ChayCodeC(7);
                        for (int j = i; j >= gap && DanhSachThamSo[j] < DanhSachThamSo[j - gap]; j -= gap)
                        {
                            bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                            bienArr["j"].Text = "j = " + j;
                            bienArr["j"].SendToBack();
                            bienArr["j"].Visible = true;
                            sapxepPanel.Controls.Add(bienArr["j"]);
                            HienThiThuatToan.ChayCodeC(9);
                            Thread.Sleep(ThamSo.ThoiGianDoi);
                            DanhSachNode[j].BackColor = DanhSachNode[j - gap].BackColor = Color.Green;
                            CapNhatThamSo(j, j - gap);
                            DichuyenCacNode(j, j - gap);
                            Hienthimangdangsapxep(i, j, "i", "j");
                        }
                    }
                    if (giamrdbtn.Checked == true)
                    {
                        HienThiThuatToan.ChayCodeC(7);
                        for (int j = i; j >= gap && DanhSachThamSo[j] > DanhSachThamSo[j - gap]; j -= gap)
                        {
                            HienThiThuatToan.ChayCodeC(9);
                            Thread.Sleep(ThamSo.ThoiGianDoi);
                            DanhSachNode[j].BackColor = DanhSachNode[j - gap].BackColor = Color.Green;
                            CapNhatThamSo(j, j - gap);
                            DichuyenCacNode(j, j - gap);
                            Hienthimangdangsapxep(i, j, "i", "j");
                        }
                    }
                }

            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();
        } // xong
        #endregion
        #region HeapSort
        private void HeapSort()
        {
            HienThiThuatToan.ChayCodeC(1);
            HienThiThuatToan.ChayCodeC(2);
            CreateHeap(DanhSachThamSo.Count);

            HienThiThuatToan.ChayCodeC(3);
            HienThiThuatToan.ChayCodeC(4);
            int r = DanhSachThamSo.Count - 1;

            HienThiThuatToan.ChayCodeC(5);
            while (r > 0)
            {
                HienThiThuatToan.ChayCodeC(6);
                HienThiThuatToan.ChayCodeC(7);
                CapNhatThamSo(0, r);
                DichuyenCacNode(r, 0);
                HienThiThuatToan.ChayCodeC(8);
                bienArr["right"].Location = new Point(danhSachLabel[r].Location.X, danhSachLabel[r].Location.Y - 160);
                bienArr["right"].Text = "right = " + r;
                sapxepPanel.Controls.Add(bienArr["right"]);
                bienArr["right"].Visible = true;
                bienArr["right"].SendToBack();
                r--;


                HienThiThuatToan.ChayCodeC(9);
                if (r > 0)
                {
                    HienThiThuatToan.ChayCodeC(10);
                    Shift(0, r);
                }

                HienThiThuatToan.ChayCodeC(11);
                HienThiThuatToan.ChayCodeC(5);
            }
            HienThiThuatToan.ChayCodeC(12);
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            if (DanhSachNode.Count != 0)
            {
                hoanTatSapXep();
            }

        }
        private void CreateHeap(int N)
        {
            HienThiThuatToan.ChayCodeC(14);
            HienThiThuatToan.ChayCodeC(15);
            HienThiThuatToan.ChayCodeC(16);
            HienThiThuatToan.ChayCodeC(17);
            int l = N / 2 - 1;

            HienThiThuatToan.ChayCodeC(18);
            while (l >= 0)
            {
                HienThiThuatToan.ChayCodeC(19);
                HienThiThuatToan.ChayCodeC(20);
                Shift(l, N - 1);

                HienThiThuatToan.ChayCodeC(21);
                bienArr["left"].Location = new Point(danhSachLabel[l].Location.X, danhSachLabel[l].Location.Y - 140);
                bienArr["left"].Text = "left = " + l;
                sapxepPanel.Controls.Add(bienArr["left"]);
                bienArr["left"].Visible = true;
                bienArr["left"].SendToBack();
                l--;

                HienThiThuatToan.ChayCodeC(22);
                HienThiThuatToan.ChayCodeC(18);
            }

            HienThiThuatToan.ChayCodeC(23);
        }

        private void Shift(int l, int r)
        {
            HienThiThuatToan.ChayCodeC(25);
            HienThiThuatToan.ChayCodeC(26);
            HienThiThuatToan.ChayCodeC(27);
            HienThiThuatToan.ChayCodeC(28);
            int i = l;
            int j = 2 * i + 1;

            HienThiThuatToan.ChayCodeC(29);
            while (j <= r)
            {
                HienThiThuatToan.ChayCodeC(30);
                HienThiThuatToan.ChayCodeC(31);
                if (tangrdbtn.Checked == true)
                {
                    if (j < r && DanhSachThamSo[j] < DanhSachThamSo[j + 1])
                    {
                        HienThiThuatToan.ChayCodeC(32);
                        j++;
                    }
                }
                else
                {
                    if (j < r && DanhSachThamSo[j] > DanhSachThamSo[j + 1])
                    {
                        HienThiThuatToan.ChayCodeC(32);
                        j++;
                    }
                }

                HienThiThuatToan.ChayCodeC(33);
                if (tangrdbtn.Checked == true)
                {
                    if (DanhSachThamSo[i] < DanhSachThamSo[j])
                    {
                        HienThiThuatToan.ChayCodeC(34);
                        HienThiThuatToan.ChayCodeC(35);

                        DichuyenCacNode(j, i);
                        CapNhatThamSo(i, j);
                        Hienthimangdangsapxep(i, j, "i", "j");


                        HienThiThuatToan.ChayCodeC(36);
                        i = j;

                        HienThiThuatToan.ChayCodeC(37);
                        j = 2 * i + 1;

                        HienThiThuatToan.ChayCodeC(38);
                    }
                    else
                    {
                        HienThiThuatToan.ChayCodeC(39);
                        return;
                    }
                }
                else
                {
                    if (DanhSachThamSo[i] > DanhSachThamSo[j])
                    {
                        HienThiThuatToan.ChayCodeC(34);
                        HienThiThuatToan.ChayCodeC(35);
                        CapNhatThamSo(i, j);
                        DichuyenCacNode(i, j);
                        Hienthimangdangsapxep(i, j, "i", "j");



                        HienThiThuatToan.ChayCodeC(36);
                        i = j;

                        HienThiThuatToan.ChayCodeC(37);
                        j = 2 * i + 1;

                        HienThiThuatToan.ChayCodeC(38);
                    }
                    else
                    {
                        HienThiThuatToan.ChayCodeC(39);
                        return;
                    }
                }

                HienThiThuatToan.ChayCodeC(40);
                HienThiThuatToan.ChayCodeC(29);
            }
            HienThiThuatToan.ChayCodeC(41);

        }

        #endregion // cần ktra lại   //cần ktra lại
        #region QuickSort
        public void QuickSort()
        {
            ThucHienQuickSort(0, DanhSachThamSo.Count - 1);
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();
        }
        private void ThucHienQuickSort(int left, int right)
        {
            HienThiThuatToan.ChayCodeC(1);
            int i, j, x, vt_x;
            bienArr["left"].Text = "left = " + left;
            bienArr["left"].Location = new Point(danhSachLabel[left].Location.X, danhSachLabel[left].Location.Y - 140);
            sapxepPanel.Controls.Add(bienArr["left"]);
            bienArr["left"].Visible = true;
            bienArr["left"].SendToBack();

            bienArr["right"].Text = "right = " + right;
            bienArr["right"].Location = new Point(danhSachLabel[right].Location.X, danhSachLabel[right].Location.Y - 160);
            sapxepPanel.Controls.Add(bienArr["right"]);
            bienArr["right"].Visible = true;
            bienArr["right"].SendToBack();
            x = DanhSachThamSo[(left + right) / 2];
            HienThiThuatToan.ChayCodeC(3);
            vt_x = (left + right) / 2;
            i = left; j = right;
            do
            {
                int z_vt_x = vt_x;
                if (tangrdbtn.Checked == true)
                {
                    HienThiThuatToan.ChayCodeC(7);
                    bienArr["vt_x"].Location = new Point(danhSachLabel[vt_x].Location.X, danhSachLabel[vt_x].Location.Y - 60);
                    bienArr["vt_x"].Text = "x = a[" + vt_x + "]";
                    sapxepPanel.Controls.Add(bienArr["vt_x"]);
                    bienArr["vt_x"].Visible = true;
                    bienArr["vt_x"].SendToBack();

                    bienArr["i"].Text = "i = " + i;
                    bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                    sapxepPanel.Controls.Add(bienArr["i"]);
                    bienArr["i"].Visible = true;
                    bienArr["i"].SendToBack();

                    bienArr["j"].Text = "j = " + j;
                    bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                    sapxepPanel.Controls.Add(bienArr["j"]);
                    bienArr["j"].Visible = true;
                    bienArr["j"].SendToBack();
                    while (DanhSachThamSo[i] < x)
                    {
                        HienThiThuatToan.ChayCodeC(8);
                        int f_i = i;
                        i++;
                        bienArr["i"].Text = "i = " + i;
                        bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                        sapxepPanel.Controls.Add(bienArr["i"]);
                        bienArr["i"].Visible = true;
                        bienArr["i"].SendToBack();
                    }
                    HienThiThuatToan.ChayCodeC(9);
                    while (DanhSachThamSo[j] > x)
                    {
                        HienThiThuatToan.ChayCodeC(10);
                        int f_j = j;
                        j--;
                        bienArr["j"].Text = "j = " + j;
                        bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                        sapxepPanel.Controls.Add(bienArr["j"]);
                        bienArr["j"].Visible = true;
                        bienArr["j"].SendToBack();
                    }
                }
                else
                {
                    HienThiThuatToan.ChayCodeC(7);
                    bienArr["vt_x"].Location = new Point(danhSachLabel[vt_x].Location.X, danhSachLabel[vt_x].Location.Y - 60);
                    bienArr["vt_x"].Text = "x = a[" + vt_x + "]";
                    sapxepPanel.Controls.Add(bienArr["vt_x"]);
                    bienArr["vt_x"].Visible = true;
                    bienArr["vt_x"].SendToBack();

                    bienArr["i"].Text = "i = " + i;
                    bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                    sapxepPanel.Controls.Add(bienArr["i"]);
                    bienArr["i"].Visible = true;
                    bienArr["i"].SendToBack();

                    bienArr["j"].Text = "j = " + j;
                    bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                    sapxepPanel.Controls.Add(bienArr["j"]);
                    bienArr["j"].Visible = true;
                    bienArr["j"].SendToBack();
                    while (DanhSachThamSo[i] > x)
                    {
                        HienThiThuatToan.ChayCodeC(8);
                        int f_i = i;
                        i++;
                        bienArr["i"].Text = "i = " + i;
                        bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                        sapxepPanel.Controls.Add(bienArr["i"]);
                        bienArr["i"].Visible = true;
                        bienArr["i"].SendToBack();
                    }
                    HienThiThuatToan.ChayCodeC(9);
                    while (DanhSachThamSo[j] < x)
                    {
                        HienThiThuatToan.ChayCodeC(10);
                        int f_j = j;
                        j--;
                        bienArr["j"].Text = "j = " + j;
                        bienArr["j"].Location = new Point(danhSachLabel[j].Location.X, danhSachLabel[j].Location.Y - 40);
                        sapxepPanel.Controls.Add(bienArr["j"]);
                        bienArr["j"].Visible = true;
                        bienArr["j"].SendToBack();
                    }
                }
                HienThiThuatToan.ChayCodeC(11);
                if (i <= j)
                {
                    int f_vt_x = vt_x;
                    if (i == vt_x)
                    {
                        vt_x = j;
                    }
                    else if (j == vt_x)
                    {
                        vt_x = i;
                    }
                    HienThiThuatToan.ChayCodeC(13);
                    CapNhatThamSo(i, j);
                    DichuyenCacNode(j, i);
                    Hienthimangdangsapxep(i, j, "i", "j");
                    bienArr["vt_x"].Location = new Point(danhSachLabel[vt_x].Location.X, danhSachLabel[vt_x].Location.Y - 60);
                    bienArr["vt_x"].Text = "x = a[" + vt_x + "]";
                    sapxepPanel.Controls.Add(bienArr["vt_x"]);
                    bienArr["vt_x"].Visible = true;
                    bienArr["vt_x"].SendToBack();
                    i++; j--;
                }

                HienThiThuatToan.ChayCodeC(16);
            } while (i <= j);
            HienThiThuatToan.ChayCodeC(17);
            if (left < j)
            {
                HienThiThuatToan.ChayCodeC(18);
                ThucHienQuickSort(left, j);
            }
            HienThiThuatToan.ChayCodeC(19);
            if (i < right)
            {
                HienThiThuatToan.ChayCodeC(20);
                ThucHienQuickSort(i, right);
            }

        }
        #endregion//xong  
        // chưa xong mảng đang xếp
        // chua cap nhat hien thi cac bien tren GUI
        #region BinaryInsectionSort
        public void BinaryInsertionSort()
        {
            yTuongTextBox.Clear();
            int left, right, m, i, pos;
            int x;
            Node nodeTam, nodeTam2;
            for (i = 1; i < DanhSachNode.Count; i++)
            {
                pos = i - 1;
                HienThiThuatToan.ChayCodeC(4);
                DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
                bienArr["i"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 20);
                bienArr["i"].Text = "i = " + i;
                sapxepPanel.Controls.Add(bienArr["i"]);
                bienArr["i"].Visible = true;
                nodeTam = DanhSachNode[i];
                x = DanhSachThamSo[i];
                HienThiThuatToan.ChayCodeC(6);
                DanhSachNode[i].ChuyenLen();
                DanhSachNode[i].BackColor = ThamSo.mauNodeDangSX;
                left = 0;
                right = i - 1;
                bienArr["left"].Location = new Point(danhSachLabel[left].Location.X, danhSachLabel[left].Location.Y - 140);
                bienArr["left"].Text = "left = " + left;
                sapxepPanel.Controls.Add(bienArr["left"]);
                //bienArr["left"].Visible = true;

                bienArr["pos"].Location = new Point(danhSachLabel[pos].Location.X, danhSachLabel[pos].Location.Y - 180);
                bienArr["pos"].Text = "pos = " + pos;
                sapxepPanel.Controls.Add(bienArr["pos"]);
                //bienArr["pos"].Visible = false;

                bienArr["right"].Location = new Point(danhSachLabel[right].Location.X, danhSachLabel[right].Location.Y - 160);
                bienArr["right"].Text = "right = " + right;
                sapxepPanel.Controls.Add(bienArr["right"]);
                //bienArr["right"].Visible = true;
                HienThiThuatToan.ChayCodeC(7);
                while (left <= right)
                {
                    m = (left + right) / 2;

                    if (tangrdbtn.Checked == true)
                    {

                        if (x < DanhSachThamSo[m])
                        {
                            HienThiThuatToan.ChayCodeC(10);
                            bienArr["m"].Location = new Point(danhSachLabel[i].Location.X, danhSachLabel[i].Location.Y - 200);
                            bienArr["m"].Text = "m = " + i;
                            sapxepPanel.Controls.Add(bienArr["m"]);
                            //bienArr["m"].Visible = true;

                            bienArr["right"].Text = "right = " + right;
                            sapxepPanel.Controls.Add(bienArr["right"]);
                           // bienArr["right"].Visible = true;

                            right = m - 1;

                            bienArr["right"].Text = "right = " + right;
                            sapxepPanel.Controls.Add(bienArr["right"]);
                           // bienArr["right"].Visible = true;
                        }
                        else
                        {
                            HienThiThuatToan.ChayCodeC(11);
                            bienArr["m"].Text = "m = " + i;
                            sapxepPanel.Controls.Add(bienArr["m"]);
                           // bienArr["m"].Visible = true;
                            left = m + 1;


                        }
                    }
                    else
                    {
                        if (x > DanhSachThamSo[m])
                        {
                            HienThiThuatToan.ChayCodeC(10);
                            bienArr["m"].Text = "m = " + i;
                            sapxepPanel.Controls.Add(bienArr["m"]);
                            //bienArr["m"].Visible = true;

                            bienArr["right"].Text = "right = " + right;
                            sapxepPanel.Controls.Add(bienArr["right"]);
                            //bienArr["right"].Visible = true;

                            right = m - 1;

                            bienArr["right"].Text = "right = " + right;
                            sapxepPanel.Controls.Add(bienArr["right"]);
                           // bienArr["right"].Visible = true;
                        }
                        else
                        {
                            HienThiThuatToan.ChayCodeC(11);
                            bienArr["m"].Text = "m = " + i;
                            sapxepPanel.Controls.Add(bienArr["m"]);
                            //bienArr["m"].Visible = true;

                            left = m + 1;


                        }
                    }
                    HienThiThuatToan.ChayCodeC(7);
                    bienArr["left"].Text = "left = " + left;
                    sapxepPanel.Controls.Add(bienArr["left"]);
                    //bienArr["left"].Visible = true;
                }
                HienThiThuatToan.ChayCodeC(13);
                bienArr["right"].Text = "right = " + right;
                sapxepPanel.Controls.Add(bienArr["right"]);
               // bienArr["right"].Visible = true;
                pos = i - 1;
                bienArr["pos"].Text = "pos = " + pos;
                sapxepPanel.Controls.Add(bienArr["pos"]);
                //bienArr["pos"].Visible = true;
                for (pos = i - 1; pos >= left; pos--)
                {
                    bienArr["pos"].Text = "pos = " + pos;
                    sapxepPanel.Controls.Add(bienArr["pos"]);
                    bienArr["pos"].Visible = true;
                    HienThiThuatToan.ChayCodeC(14);
                    DanhSachNode[pos].ChuyenNgang(pos + 1);
                    DanhSachNode[pos].vitriHienTai = pos + 1;
                    nodeTam2 = DanhSachNode[pos + 1];
                    DanhSachNode[pos + 1] = DanhSachNode[pos];
                    DanhSachThamSo[pos + 1] = DanhSachThamSo[pos];
                    DanhSachNode[pos] = nodeTam2;
                }

                bienArr["pos"].Text = "pos = " + pos;
                sapxepPanel.Controls.Add(bienArr["pos"]);
                bienArr["pos"].Visible = true;
                HienThiThuatToan.ChayCodeC(15);

                nodeTam.ChuyenNgang(pos + 1);
                nodeTam.ChuyenXuong();
                DanhSachNode[pos + 1] = nodeTam;
                nodeTam.vitriHienTai = pos + 1;
                DanhSachNode[pos+1].BackColor = ThamSo.mauNodeHTSX;

                DanhSachThamSo[left] = x;

                //Cập nhật ý tưởng Text Box
                Hienthimangdangsapxep(pos, i, "pos", "i");


            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();
        }

        #endregion
        // chưa xong mảng đang xếp
        //mergesoft t chua coi ky nen k hieu y tuong thuat toan voi cho nao cap nhat lai danh sach tham so
        // se bo xung file huong dan y tuong thuat toan sau
        #region MergeSoft
        List<Node> b = new List<Node>();
        List<Node> c = new List<Node>();
        int nb, nc;

        int Min(int a, int b)
        {
            if (a > b) return b;
            else return a;
        }
        void Distribute(List<Node> a, int N, ref int nb, ref int nc, int k)
        {
            int i, pa, pb, pc;
            pa = pb = pc = 0;
            HienThiThuatToan.ChayCodeC(10);

            while (pa < N)
            {
                HienThiThuatToan.ChayCodeC(12);
                for (i = 0; (pa < N) && (i < k); i++, pa++, pb++)
                {

                    //b[pb] = a[pa];
                    HienThiThuatToan.ChayCodeC(13);
                    a[pa].BackColor = ThamSo.mauNodeDangSX;
                    a[pa].ChuyenLen();
                    a[pa].ChuyenNgang(pb);
                    a[pa].vitriHienTai = pb;

                    b[pb] = a[pa];
                    HienThiThuatToan.ChayCodeC(12);
                }
                HienThiThuatToan.ChayCodeC(12);
                HienThiThuatToan.ChayCodeC(14);
                for (i = 0; (pa < N) && (i < k); i++, pa++, pc++)
                {
                    HienThiThuatToan.ChayCodeC(15);
                    //c[pc] = a[pa];
                    a[pa].BackColor = ThamSo.mauNodeDangSX;
                    a[pa].ChuyenXuong();
                    a[pa].ChuyenNgang(pc);
                    a[pa].vitriHienTai = pc;

                    c[pc] = a[pa];

                    HienThiThuatToan.ChayCodeC(14);
                }
                HienThiThuatToan.ChayCodeC(14);
                HienThiThuatToan.ChayCodeC(10);
            }
            HienThiThuatToan.ChayCodeC(10);
            nb = pb; nc = pc;
        }
        void Merge(List<Node> a, int nb, int nc, int k)
        {
            int p, pb, pc, ib, ic, kb, kc;
            p = pb = pc = 0; ib = ic = 0;
            HienThiThuatToan.ChayCodeC(23);

            while ((nb > 0) && (nc > 0))
            {
                kb = Min(k, nb);
                kc = Min(k, nc);
                HienThiThuatToan.ChayCodeC(26);

                bool thucHien = false; // dùng để xét tăng/giảm , nếu bằng true thì code sẽ chạy
                if (tangrdbtn.Checked == true)
                {
                    if (c[pc + ic].Value >= b[pb + ib].Value)
                        thucHien = true;
                }
                else
                {
                    if (c[pc + ic].Value <= b[pb + ib].Value)
                        thucHien = true;
                }
                if (thucHien)
                {

                    //a[p++] = b[pb + ib];
                    HienThiThuatToan.ChayCodeC(28);
                    b[pb + ib].BackColor = ThamSo.mauNen;
                    b[pb + ib].ChuyenXuong();
                    b[pb + ib].ChuyenNgang(p);
                    b[pb + ib].vitriHienTai = p;

                    a[p] = b[pb + ib];
                    p = p + 1;

                    ib++;

                    HienThiThuatToan.ChayCodeC(29);
                    if (ib == kb)
                    {
                        HienThiThuatToan.ChayCodeC(31);
                        for (; ic < kc; ic++)
                        {
                            HienThiThuatToan.ChayCodeC(32);
                            //a[p++] = c[pc + ic];
                            c[pc + ic].BackColor = ThamSo.mauNen;
                            c[pc + ic].ChuyenLen();
                            c[pc + ic].ChuyenNgang(p);
                            c[pc + ic].vitriHienTai = p;

                            a[p] = c[pc + ic];
                            p = p + 1;
                            HienThiThuatToan.ChayCodeC(32);
                        }

                        pb += kb; pc += kc; ib = ic = 0;
                        nb -= kb; nc -= kc;
                    }
                }
                else
                {
                    HienThiThuatToan.ChayCodeC(39);
                    //a[p++] = c[pc + ic];
                    c[pc + ic].BackColor = ThamSo.mauNen;
                    c[pc + ic].ChuyenLen();
                    c[pc + ic].ChuyenNgang(p);
                    c[pc + ic].vitriHienTai = p;

                    a[p] = c[pc + ic];
                    p = p + 1;

                    ic++;
                    HienThiThuatToan.ChayCodeC(40);
                    if (ic == kc)
                    {
                        HienThiThuatToan.ChayCodeC(42);
                        for (; ib < kb; ib++)
                        {
                            HienThiThuatToan.ChayCodeC(43);
                            //a[p++] = b[pb + ib];
                            b[pb + ib].BackColor = ThamSo.mauNen;
                            b[pb + ib].ChuyenXuong();
                            b[pb + ib].ChuyenNgang(p);
                            b[pb + ib].vitriHienTai = p;


                            a[p] = b[pb + ib];
                            p = p + 1;
                            HienThiThuatToan.ChayCodeC(43);
                        }
                        HienThiThuatToan.ChayCodeC(42);
                        pb += kb; pc += kc; ib = ic = 0;
                        nb -= kb; nc -= kc;
                    }
                }

                HienThiThuatToan.ChayCodeC(23);
            }  // while
            HienThiThuatToan.ChayCodeC(23);
            if (a.Count % 2 == 1 && (k != (a.Count - 1)))
            {
                if (nb > nc)
                {
                    HienThiThuatToan.ChayCodeC(43);
                    b[pb].BackColor = ThamSo.mauNen;
                    b[pb].ChuyenXuong();
                    b[pb].ChuyenNgang(a.Count - 1);
                    b[pb].vitriHienTai = a.Count - 1;

                }

            }
            if (a.Count % 2 == 0 && Math.Abs(nb - nc) == 2)
            {

                HienThiThuatToan.ChayCodeC(43);
                b[pb].BackColor = ThamSo.mauNen;
                b[pb].ChuyenXuong();
                b[pb].ChuyenNgang(a.Count - 2);
                b[pb].vitriHienTai = a.Count - 2;
                HienThiThuatToan.ChayCodeC(43);
                b[pb + 1].BackColor = ThamSo.mauNen;
                b[pb + 1].ChuyenXuong();
                b[pb + 1].ChuyenNgang(a.Count - 1);
                b[pb + 1].vitriHienTai = a.Count - 1;


            }

            ;
        }
        void ThucHienMergeSort(List<Node> a, int N)
        {
            for (int i = 0; i < SoLuongNode; i++)
            {
                b.Add(new Node(i));
                c.Add(new Node(i));
            }
            int k;
            HienThiThuatToan.ChayCodeC(53);
            for (k = 1; k < N; k *= 2)
            {
                bienArr["k"].Location = new Point(danhSachLabel[k].Location.X, danhSachLabel[k].Location.Y - 200);
                bienArr["k"].Text = "k = " + k;
                sapxepPanel.Controls.Add(bienArr["k"]);
                bienArr["k"].Visible = true;
                HienThiThuatToan.ChayCodeC(55);
                Distribute(a, N, ref nb, ref nc, k);
                HienThiThuatToan.ChayCodeC(56);
                Merge(a, nb, nc, k);
                HienThiThuatToan.ChayCodeC(53);
            }
            HienThiThuatToan.ChayCodeC(53);
        }
        void MergeSort()
        {
            yTuongTextBox.Clear();
            bienArr["a:"].Location = new Point(0, 110 + DanhSachNode[0].Height / 2);
            bienArr["a:"].Text = "a:";
            sapxepPanel.Controls.Add(bienArr["a:"]);
            bienArr["c:"].Location = new Point(0, 160 + DanhSachNode[0].Height / 2);
            bienArr["c:"].Text = "c:";
            sapxepPanel.Controls.Add(bienArr["c:"]);
            bienArr["b:"].Location = new Point(0, 60 + DanhSachNode[0].Height / 2);
            bienArr["b:"].Text = "b:";
            sapxepPanel.Controls.Add(bienArr["b:"]);
            bienArr["a:"].Visible = bienArr["b:"].Visible = bienArr["c:"].Visible = true;
            HienThiThuatToan.ChayCodeC(51);
            ThucHienMergeSort(DanhSachNode, DanhSachNode.Count);
            foreach (Node node in DanhSachNode)
            {
                node.BackColor = ThamSo.mauNodeHTSX;
            }
            DanhSachNode[0].BackColor = ThamSo.mauNodeHTSX;
            DanhSachNode[DanhSachNode.Count - 1].BackColor = ThamSo.mauNodeHTSX;
            hoanTatSapXep();

        }


        #endregion
        // chưa xong mảng đang xếp



        private void lbDieuKhien_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ThamSo.ThoiGianDoi = trackBar1.Value;
            label1.Text = trackBar1.Value.ToString();
        }

        private void huybnt_Click(object sender, EventArgs e)
        {

            sapxepThread.Abort();
            VeNut();
            isRunning = false;
            for (int i = 0; i < SoLuongNode; i++)
            {
                DanhSachNode[i].Text = MangChuaSapXep[i].ToString();
                DanhSachThamSo[i] = MangChuaSapXep[i];
            }
            DieuChinhControls(isRunning);
            Mangchuasapxep();
            Reset_CountTime();
            timer1.Stop();
            huybnt.Enabled = false;
        }


        private void Mainform_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (sapxepThread != null)
            {
                sapxepThread.Abort();
            }


        }

        private void sapxepPanel_MouseClick(object sender, MouseEventArgs e)
        { 
                batdaubtn.Focus();  
        }

        private void thoatbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xoamangbtn_Click(object sender, EventArgs e)
        {
            sapxepPanel.Controls.Clear();
            DanhSachNode.Clear();
            DanhSachThamSo.Clear();
            chuasapxepPanel.Controls.Clear();
            batdaubtn.Enabled = false;
       
        }

        private void khoitaopanel_Paint(object sender, PaintEventArgs e)
        {
         
        }

        private void nhaptaybtn_Click(object sender, EventArgs e)
        {
            frmInput input = new frmInput();
            input.ShowDialog();
        }

        private void Mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
