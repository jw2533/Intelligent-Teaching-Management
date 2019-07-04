using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;//数据流和文件操作
using System.Net.Mail;
using System.Net;


namespace 智慧教学管理系统
{
	public partial class Form1 : Form
	{
		FileStream fsIn, fsOut;
		StreamReader sr;
		StreamWriter sw;
		string path;

		Dictionary<int, Xuesheng> xueDict;
		List<Xuesheng> xueList;
		Dictionary<int, Laoshi> laoDict;
		List<Laoshi> laoList;
		List<Kecheng> keList;
		DataTable dt;
		Xuesheng curStu;
		Dictionary<string, int> xueKeFenDict;
		Laoshi curTea;
		List<Kecheng> laoKeList;
		List<Zuoye> hwList;
		Kecheng curKe;
		List<Dayi> ansList;
		
		public Form1()
		{
			string str;
			string[] arr;

			InitializeComponent();
			path = Environment.CurrentDirectory;
			gpbManage.Visible = false;
			gpbStu.Visible = false;
			gpbTea.Visible = false;
			gpbNotice.Visible = false;

			//加载学生信息
			fsIn = new FileStream(path + "\\student.txt", FileMode.Open, FileAccess.Read);
			sr = new StreamReader(fsIn);
			xueDict = new Dictionary<int, Xuesheng>();
			xueList = new List<Xuesheng>();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				xueDict[int.Parse(arr[0])] = new Xuesheng(int.Parse(arr[0]), arr[1], arr[2], int.Parse(arr[3]), int.Parse(arr[4]), arr[5], arr[6], int.Parse(arr[7]), float.Parse(arr[8]), int.Parse(arr[9]), arr[10], int.Parse(arr[11]));
				xueList.Add(xueDict[int.Parse(arr[0])]);
			}
			fsIn.Close();
			sr.Close();

			//加载教师信息
			fsIn = new FileStream(path + "\\teacher.txt", FileMode.Open, FileAccess.Read);
			sr = new StreamReader(fsIn);
			laoDict = new Dictionary<int, Laoshi>();
			laoList = new List<Laoshi>();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				laoDict[int.Parse(arr[0])] = new Laoshi(int.Parse(arr[0]), arr[1], arr[2], int.Parse(arr[3]), arr[4], arr[5], arr[6], float.Parse(arr[7]));
				laoList.Add(laoDict[int.Parse(arr[0])]);
			}
			fsIn.Close();
			sr.Close();

			//加载课程信息
			fsIn = new FileStream(path + "\\course.txt", FileMode.Open, FileAccess.Read);
			sr = new StreamReader(fsIn);
			keList = new List<Kecheng>();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				Kecheng cou= new Kecheng(int.Parse(arr[0]), arr[1], int.Parse(arr[2]), int.Parse(arr[3]), int.Parse(arr[4]), int.Parse(arr[5]));
				cou.homework = int.Parse(arr[6]);
				keList.Add(cou);
			}
			fsIn.Close();
			sr.Close();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//保存学生信息
			fsOut = new FileStream(path + "\\student.txt", FileMode.Create, FileAccess.Write);
			sw = new StreamWriter(fsOut);
			foreach (Xuesheng st in xueList)
			{
				sw.WriteLine(st.id.ToString() + ' ' + st.name + ' ' + st.sex + ' ' + st.age + ' ' + st.grade + ' ' + st.faculty + ' ' + st.major + ' ' + st.classes + ' ' + st.GPA + ' ' + st.attendance + ' ' + st.prize + ' ' + st.exchange);
			}
			sw.Flush();
			sw.Close();
			fsOut.Close();

			//保存教师信息
			fsOut = new FileStream(path + "\\teacher.txt", FileMode.Create, FileAccess.Write);
			sw = new StreamWriter(fsOut);
			foreach (Laoshi tr in laoList)
			{
				sw.WriteLine(tr.id.ToString() + ' ' + tr.name + ' ' + tr.sex + ' ' + tr.age + ' ' + tr.faculty + ' ' + tr.major + ' ' + tr.title + ' ' + tr.appraise);
			}
			sw.Flush();
			sw.Close();
			fsOut.Close();

			//保存课程信息
			fsOut = new FileStream(path + "\\course.txt", FileMode.Create, FileAccess.Write);
			sw = new StreamWriter(fsOut);
			foreach (Kecheng cou in keList)
			{
				sw.WriteLine(cou.id.ToString() + ' ' + cou.name + ' ' + cou.teacher1 + ' ' + cou.teacher2 + ' ' + cou.teacher3 + ' ' + cou.finish + ' ' + cou.homework);
			}
			sw.Flush();
			sw.Close();
			fsOut.Close();
		}

		private void btnStudentUpdate_Click(object sender, EventArgs e)
		{
			dt = dgv.DataSource as DataTable;
			xueDict.Clear();
			xueList.Clear();
			for ( int i = 0; i < dt.Rows.Count; i++ )
			{
				DataRow dr = dt.Rows[i];
				int id = int.Parse(dr[0].ToString());
				string name = dr[1].ToString();
				string sex = dr[2].ToString();
				int age = int.Parse(dr[3].ToString());
				int grade = int.Parse(dr[4].ToString());
				string faculty = dr[5].ToString();
				string major = dr[6].ToString();
				int classes = int.Parse(dr[7].ToString());
				float GPA = float.Parse(dr[8].ToString());
				int attendance = int.Parse(dr[9].ToString());
				string prize = dr[10].ToString();
				int exchange = int.Parse(dr[11].ToString());
				xueDict[id] = new Xuesheng(id, name, sex, age, grade, faculty, major, classes, GPA, attendance, prize, exchange);
				xueList.Add(xueDict[id]);
			}
		}


		private void ckbStuSeek_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void btnStuSeek_Click(object sender, EventArgs e)
		{
			DataTable datatable = new DataTable();
			List<Xuesheng> resultList = new List<Xuesheng>();
			switch(cbbStuSeek.SelectedIndex)
			{
				case 0:
					int id = int.Parse(txtStuSeek.Text);
					foreach( Xuesheng st in xueList )
					{
						if( st.id == id )
						{
							resultList.Add(st);
						}
					}
					break;
				case 1:
					string name = txtStuSeek.Text;
					foreach (Xuesheng st in xueList)
					{
						if (st.name.Equals(name))
						{
							resultList.Add(st);
						}
					}
					break;
				case 2:
					string sex = txtStuSeek.Text;
					foreach (Xuesheng st in xueList)
					{
						if (st.sex.Equals(sex))
						{
							resultList.Add(st);
						}
					}
					break;
				case 3:
					int age = int.Parse(txtStuSeek.Text);
					foreach (Xuesheng st in xueList)
					{
						if (st.age == age)
						{
							resultList.Add(st);
						}
					}
					break;
				case 4:
					int grade = int.Parse(txtStuSeek.Text);
					foreach (Xuesheng st in xueList)
					{
						if (st.grade == grade)
						{
							resultList.Add(st);
						}
					}
					break;
				case 5:
					string faculty = txtStuSeek.Text;
					foreach (Xuesheng st in xueList)
					{
						if (st.faculty.Equals(faculty))
						{
							resultList.Add(st);
						}
					}
					break;
				case 6:
					string major = txtStuSeek.Text;
					foreach (Xuesheng st in xueList)
					{
						if (st.major.Equals(major))
						{
							resultList.Add(st);
						}
					}
					break;
				case 7:
					int classes = int.Parse(txtStuSeek.Text);
					foreach (Xuesheng st in xueList)
					{
						if (st.classes == classes)
						{
							resultList.Add(st);
						}
					}
					break;
				case 8:
					float GPA = int.Parse(txtStuSeek.Text);
					foreach (Xuesheng st in xueList)
					{
						if (st.GPA == GPA)
						{
							resultList.Add(st);
						}
					}
					break;
				default:
					break;
			}
			datatable.Columns.Add(new DataColumn("学号", typeof(int)));
			datatable.Columns.Add(new DataColumn("姓名", typeof(string)));
			datatable.Columns.Add(new DataColumn("性别", typeof(string)));
			datatable.Columns.Add(new DataColumn("年龄", typeof(int)));
			datatable.Columns.Add(new DataColumn("年级", typeof(int)));
			datatable.Columns.Add(new DataColumn("院系", typeof(string)));
			datatable.Columns.Add(new DataColumn("专业", typeof(string)));
			datatable.Columns.Add(new DataColumn("班级", typeof(int)));
			datatable.Columns.Add(new DataColumn("绩点", typeof(float)));
			datatable.Columns.Add(new DataColumn("本月出勤天数", typeof(int)));
			datatable.Columns.Add(new DataColumn("竞赛获奖", typeof(string)));
			datatable.Columns.Add(new DataColumn("学术交流次数", typeof(int)));
			for (int i = 0; i < resultList.Count; i++)
			{
				DataRow dr = datatable.NewRow();
				dr[0] = resultList[i].id;
				dr[1] = resultList[i].name;
				dr[2] = resultList[i].sex;
				dr[3] = resultList[i].age;
				dr[4] = resultList[i].grade;
				dr[5] = resultList[i].faculty;
				dr[6] = resultList[i].major;
				dr[7] = resultList[i].classes;
				dr[8] = resultList[i].GPA;
				dr[9] = resultList[i].attendance;
				dr[10] = resultList[i].prize;
				dr[11] = resultList[i].exchange;
				datatable.Rows.Add(dr);
			}
			dgv.DataSource = datatable;
		}
		

		private void btnTeaUpdate_Click(object sender, EventArgs e)
		{
			dt = dgv.DataSource as DataTable;
			laoDict.Clear();
			laoList.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				int id = int.Parse(dr[0].ToString());
				string name = dr[1].ToString();
				string sex = dr[2].ToString();
				int age = int.Parse(dr[3].ToString());
				string faculty = dr[4].ToString();
				string major = dr[5].ToString();
				string title = dr[6].ToString();
				float appraise = float.Parse(dr[7].ToString());
				laoDict[id] = new Laoshi(id, name, sex, age, faculty, major, title, appraise);
				laoList.Add(laoDict[id]);
			}
		}
		
		private void btnTeaSeek_Click(object sender, EventArgs e)
		{
			DataTable datatable = new DataTable();
			List<Laoshi> resultList = new List<Laoshi>();
			switch (cbbTeaSeek.SelectedIndex)
			{
				case 0:
					int id = int.Parse(txtTeaSeek.Text);
					foreach (Laoshi st in laoList)
					{
						if (st.id == id)
						{
							resultList.Add(st);
						}
					}
					break;
				case 1:
					string name = txtTeaSeek.Text;
					foreach (Laoshi st in laoList)
					{
						if (st.name.Equals(name))
						{
							resultList.Add(st);
						}
					}
					break;
				case 2:
					string sex = txtTeaSeek.Text;
					foreach (Laoshi st in laoList)
					{
						if (st.sex.Equals(sex))
						{
							resultList.Add(st);
						}
					}
					break;
				case 3:
					int age = int.Parse(txtTeaSeek.Text);
					foreach (Laoshi st in laoList)
					{
						if (st.age == age)
						{
							resultList.Add(st);
						}
					}
					break;
				case 4:
					int faculty = int.Parse(txtTeaSeek.Text);
					foreach (Laoshi st in laoList)
					{
						if (st.faculty.Equals(faculty))
						{
							resultList.Add(st);
						}
					}
					break;
				case 5:
					string major = txtTeaSeek.Text;
					foreach (Laoshi st in laoList)
					{
						if (st.major.Equals(major))
						{
							resultList.Add(st);
						}
					}
					break;
				case 6:
					string title = txtTeaSeek.Text;
					foreach (Laoshi st in laoList)
					{
						if (st.title.Equals(title))
						{
							resultList.Add(st);
						}
					}
					break;
				case 7:
					float appraise = float.Parse(txtTeaSeek.Text);
					foreach (Laoshi st in laoList)
					{
						if (st.appraise == appraise)
						{
							resultList.Add(st);
						}
					}
					break;
				default:
					break;
			}
			datatable.Columns.Add(new DataColumn("工号", typeof(int)));
			datatable.Columns.Add(new DataColumn("姓名", typeof(string)));
			datatable.Columns.Add(new DataColumn("性别", typeof(string)));
			datatable.Columns.Add(new DataColumn("年龄", typeof(int)));
			datatable.Columns.Add(new DataColumn("院系", typeof(string)));
			datatable.Columns.Add(new DataColumn("专业", typeof(string)));
			datatable.Columns.Add(new DataColumn("职称", typeof(string)));
			datatable.Columns.Add(new DataColumn("评教分数", typeof(float)));
			for (int i = 0; i < resultList.Count; i++)
			{
				DataRow dr = datatable.NewRow();
				dr[0] = resultList[i].id;
				dr[1] = resultList[i].name;
				dr[2] = resultList[i].sex;
				dr[3] = resultList[i].age;
				dr[4] = resultList[i].faculty;
				dr[5] = resultList[i].major;
				dr[6] = resultList[i].title;
				dr[7] = resultList[i].appraise;
				datatable.Rows.Add(dr);
			}
			dgv.DataSource = datatable;
		}

		private void btnCourseUpdata_Click(object sender, EventArgs e)
		{
			dt = dgv.DataSource as DataTable;
			List<Kecheng> tempList = new List<Kecheng>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				int id = int.Parse(dr[0].ToString());
				string name = dr[1].ToString();
				string tea1 = dr[2].ToString();
				string tea2 = dr[3].ToString();
				string tea3 = dr[4].ToString();
				int t1 = -1, t2 = -1, t3 = -1;
				foreach (Laoshi tr in laoList)
				{
					if( tr.name.Equals(tea1) )
					{
						t1 = tr.id;
					}
					else if(tr.name.Equals(tea2))
					{
						t2 = tr.id;
					}
					else if (tr.name.Equals(tea3))
					{
						t3 = tr.id;
					}
				}
				if(t1 == -1)
				{
					MessageBox.Show("教师“" + tea1 + "”不存在，本次更新失败，请重新确认！");
					return;
				}
				if (t2 == -1)
				{
					MessageBox.Show("教师“" + tea2 + "”不存在，本次更新失败，请重新确认！");
					return;
				}
				if (t2 == -1)
				{
					MessageBox.Show("教师“" + tea2 + "”不存在，本次更新失败，请重新确认！");
					return;
				}
				string finish = dr[5].ToString();
				tempList.Add(new Kecheng(id, name, t1, t2, t3, finish.Equals("是")?1:0));
			}
			keList = tempList;
		}

		private void ckbCourseSeek_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void btnCourseSeek_Click(object sender, EventArgs e)
		{
			DataTable datatable = new DataTable();
			List<Kecheng> resultList = new List<Kecheng>();
			switch (cbbCourseSeek.SelectedIndex)
			{
				case 0:
					int id = int.Parse(txtCourseSeek.Text);
					foreach (Kecheng st in keList)
					{
						if (st.id == id)
						{
							resultList.Add(st);
						}
					}
					break;
				case 1:
					string name = txtCourseSeek.Text;
					foreach (Kecheng st in keList)
					{
						if (st.name.Equals(name))
						{
							resultList.Add(st);
						}
					}
					break;
				case 2:
					string t = txtCourseSeek.Text;
					int t_id = -1;
					foreach(Laoshi tr in laoList)
					{
						if(tr.name.Equals(t))
						{
							t_id = tr.id;
							break;
						}
					}
					if( t_id != -1 )
					{
						foreach (Kecheng st in keList)
						{
							if (t_id == st.teacher1)
							{
								resultList.Add(st);
							}
							else if (t_id == st.teacher2)
							{
								resultList.Add(st);
							}
							else if (t_id == st.teacher3)
							{
								resultList.Add(st);
							}
						}
					}
					break;
				default:
					break;
			}
			datatable = new DataTable();
			datatable.Columns.Add(new DataColumn("课程号", typeof(int)));
			datatable.Columns.Add(new DataColumn("课程名", typeof(string)));
			datatable.Columns.Add(new DataColumn("教师1", typeof(string)));
			datatable.Columns.Add(new DataColumn("教师2", typeof(string)));
			datatable.Columns.Add(new DataColumn("教师3", typeof(string)));
			datatable.Columns.Add(new DataColumn("是否结课", typeof(string)));
			for (int i = 0; i < resultList.Count; i++)
			{
				DataRow dr = datatable.NewRow();
				dr[0] = resultList[i].id;
				dr[1] = resultList[i].name;
				dr[2] = laoDict[resultList[i].teacher1].name;
				dr[3] = laoDict[resultList[i].teacher2].name;
				dr[4] = laoDict[resultList[i].teacher3].name;
				dr[5] = (resultList[i].finish == 1)?"是":"否";
				datatable.Rows.Add(dr);
			}
			dgv.DataSource = datatable;
		}

		private void btnCourseSource_Click(object sender, EventArgs e)
		{
			DataRow dr;
			//先对数据进行更新
			dt = dgv.DataSource as DataTable;
			List<Kecheng> tempList = new List<Kecheng>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dr = dt.Rows[i];
				int id = int.Parse(dr[0].ToString());
				string name = dr[1].ToString();
				string tea1 = dr[2].ToString();
				string tea2 = dr[3].ToString();
				string tea3 = dr[4].ToString();
				int t1 = -1, t2 = -1, t3 = -1;
				foreach (Laoshi tr in laoList)
				{
					if (tr.name.Equals(tea1))
					{
						t1 = tr.id;
					}
					else if (tr.name.Equals(tea2))
					{
						t2 = tr.id;
					}
					else if (tr.name.Equals(tea3))
					{
						t3 = tr.id;
					}
				}
				if (t1 == -1)
				{
					MessageBox.Show("教师“" + tea1 + "”不存在，本次更新失败，请重新确认！");
					rbCourseUpdate.Checked = true;
					return;
				}
				if (t2 == -1)
				{
					MessageBox.Show("教师“" + tea2 + "”不存在，本次更新失败，请重新确认！");
					rbCourseUpdate.Checked = true;
					return;
				}
				if (t2 == -1)
				{
					MessageBox.Show("教师“" + tea2 + "”不存在，本次更新失败，请重新确认！");
					rbCourseUpdate.Checked = true;
					return;
				}
				string finish = dr[5].ToString();
				tempList.Add(new Kecheng(id, name, t1, t2, t3, finish.Equals("是") ? 1 : 0));
			}
			keList = tempList;

			rbCourseUpdate.Enabled = false;
			rbCourseSeek.Enabled = false;
			btnCourseUpdata.Enabled = false;
			dgv.ReadOnly = true;
			cbbCourseSeek.Enabled = false;
			txtCourseSeek.Enabled = false;
			btnCourseSeek.Enabled = false;
			cbbCourseSeek.SelectedIndex = 0;
			btnCourseSource.Enabled = false;
			btnCourseBack.Enabled = true;

			dt = dgv.DataSource as DataTable;
			dr = dt.Rows[dgv.CurrentCell.RowIndex];
		//	int c_id = int.Parse(dr[0].ToString());
			string courseNmae = dr[1].ToString();
			string str;
			string[] arr;
			DataTable datatable = new DataTable();
			datatable.Columns.Add(new DataColumn("课程名", typeof(string)));
			datatable.Columns.Add(new DataColumn("学生姓名", typeof(string)));
			datatable.Columns.Add(new DataColumn("专业", typeof(string)));
			datatable.Columns.Add(new DataColumn("成绩", typeof(int)));
			//读取成绩
			fsIn = new FileStream(path + "\\" + courseNmae, FileMode.OpenOrCreate, FileAccess.Read);
			sr = new StreamReader(fsIn);
		//	Dictionary<int, int> courseDit = new Dictionary<int, int>();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				DataRow drow = datatable.NewRow();
				int s_id = int.Parse(arr[0]);
				drow[0] = courseNmae;                                          //会报错
				drow[1] = xueDict[s_id].name;
				drow[2] = xueDict[s_id].major;
				drow[3] = int.Parse(arr[1]);
				datatable.Rows.Add(drow);
			}
			dgv.DataSource = datatable;
			dgv.Columns[0].Width = dgv.Columns[1].Width = dgv.Columns[2].Width = dgv.Columns[3].Width = 225;
			fsIn.Close();
			sr.Close();
		}

		private void btnCourseBack_Click(object sender, EventArgs e)
		{
			dgv.DataSource = dt;
			btnCourseBack.Enabled = false;
			btnCourseSource.Enabled = true;
			rbCourseUpdate.Enabled = true;
			rbCourseSeek.Enabled = true;
		}
		private void btnStuLogin_Click(object sender, EventArgs e)
		{
			try
			{
				int id = int.Parse(txtStuLogin.Text);
				if( xueDict.ContainsKey(id) )
				{
					curStu = xueDict[id];
					gpbStuCourse.Enabled = true;
					btnStuSelectCourseComfrim.Visible = false;
					cbbStuSelectCourse.Visible = false;
					btnStuCancellCourse.Enabled = false;
					rtxtStuAsk.Enabled = false;
					btnStuAsk.Enabled = false;
					rtxtStuHomekorkAsw.Enabled = false;
					btnStuHomekorkUp.Enabled = false;
					lblStuID.Text = curStu.id.ToString();
					lblStuName.Text = curStu.name;
					lblStuGrade.Text = curStu.grade.ToString();
					lblStuFaculty.Text = curStu.faculty;
					lblStuMajor.Text = curStu.major;
					lblStuClass.Text = curStu.classes.ToString();
					lblStuGPA.Text = curStu.GPA.ToString();
					rtxtStuAsk.Clear();
					rtxtStuAnswer.Clear();
					cbbStuCourse.Items.Clear();
					cbbStuSelectCourse.Items.Clear();
					xueKeFenDict = new Dictionary<string, int>();
					foreach(Kecheng cou in keList)
					{
						fsIn = new FileStream(path + "\\" + cou.name, FileMode.OpenOrCreate, FileAccess.Read);
						sr = new StreamReader(fsIn);
						string str;
						string[] arr;
						//	Dictionary<int, int> courseDit = new Dictionary<int, int>();
						bool flag = false;
						while (sr.Peek() >= 0)//找已选的课
						{
							str = sr.ReadLine();
							arr = str.Split(' ');
							if( arr[0].Equals(lblStuID.Text) )
							{
								xueKeFenDict[cou.name] = int.Parse(arr[1]);
								cbbStuCourse.Items.Add(cou.name);
								flag = true;
								break;
							}
						}
						if( flag == false )//可选的课
						{
							if( cou.finish == 0 )
							{
								cbbStuSelectCourse.Items.Add(cou.name);
							}
						}
						fsIn.Close();
						sr.Close();
					}
				}
				else
				{
					MessageBox.Show("该学号不存在！");
				}
			}
			catch
			{
				MessageBox.Show("输入的学号信息有误！");
			}
		}

		private void lbStuCourse_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void btnStuCancellCourse_Click(object sender, EventArgs e)
		{
			//读取课程信息，删除该学生
			string couName = cbbStuCourse.SelectedItem.ToString();
			fsIn = new FileStream(path + "\\" + couName, FileMode.OpenOrCreate, FileAccess.Read);
			sr = new StreamReader(fsIn);
			string str;
			string[] arr;
			Dictionary<int, int> courseDit = new Dictionary<int, int>();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				courseDit[int.Parse(arr[0])] = int.Parse(arr[1]);
			}
			fsIn.Close();
			sr.Close();
			courseDit.Remove(curStu.id);
			//保存新课程信息
			fsOut = new FileStream(path + "\\" + couName, FileMode.Create, FileAccess.Write);
			sw = new StreamWriter(fsOut);
			foreach (int key in courseDit.Keys)
			{
				sw.WriteLine( key.ToString() + ' ' + courseDit[key].ToString() );
			}
			sw.Flush();
			sw.Close();
			fsOut.Close();
			//更新界面
			cbbStuCourse.SelectedIndex = 0;
			cbbStuCourse.Items.Remove(couName);
			cbbStuSelectCourse.Items.Add(couName);
		}

		private void ckbStuSelectCourse_CheckedChanged(object sender, EventArgs e)
		{
			cbbStuSelectCourse.Visible = ckbStuSelectCourse.Checked;
			btnStuSelectCourseComfrim.Visible = ckbStuSelectCourse.Checked;
		}

		private void btnStuSelectCourseComfrim_Click(object sender, EventArgs e)
		{
			string couName = "";
			try
			{
				couName = cbbStuSelectCourse.SelectedItem.ToString();
			}
			catch
			{
				MessageBox.Show("请选择课程！");
				return;
			}
			//追加学生信息
			fsOut = new FileStream(path + "\\" + couName, FileMode.Append);
			sw = new StreamWriter(fsOut);
			sw.WriteLine(curStu.id.ToString() + " 0");
			sw.Flush();
			sw.Close();
			fsOut.Close();
			//更新界面
			cbbStuCourse.Items.Add(couName);
			cbbStuSelectCourse.Items.Remove(couName);
			MessageBox.Show("选课成功！");
		}

		private void btnTeaLogin_Click(object sender, EventArgs e)
		{
			try
			{
				int id = int.Parse(txtTeaLogin.Text);
				if (laoDict.ContainsKey(id))
				{
					curTea = laoDict[id];
					gpbTeaCourse.Enabled = true;
					ckbTeaCouFinish.Enabled = false;
					rbHomeworkClose.Enabled = rbHomeworkOpen.Enabled = false;
					txtTeaHomeworkSource.Enabled = false;
					btnTeaHomeworkFinish.Enabled = false;
					lbAskStu.Enabled = false;
					rtxtTeaAnswer.Enabled = false;
					btnTeaAnswer.Enabled = false;
					lblTeaID.Text = curTea.id.ToString();
					lblTeaName.Text = curTea.name;
					lblTeaTitle.Text = curTea.title;
					lblTeaFaculty.Text = curTea.faculty;
					lblTeaMajor.Text = curTea.major;
					lblTeaAppraise.Text = curTea.appraise.ToString();

					cbbTeaCourse.Items.Clear();
					laoKeList = new List<Kecheng>();
					foreach (Kecheng cou in keList)
					{
						if(cou.teacher1 == curTea.id)
						{
							laoKeList.Add(cou);
							cbbTeaCourse.Items.Add(cou.name);
						}
						else if (cou.teacher2 == curTea.id)
						{
							laoKeList.Add(cou);
							cbbTeaCourse.Items.Add(cou.name);
						}
						else if (cou.teacher3 == curTea.id)
						{
							laoKeList.Add(cou);
							cbbTeaCourse.Items.Add(cou.name);
						}
					}
				}
				else
				{
					MessageBox.Show("该工号不存在！");
				}
			}
			catch
			{
				MessageBox.Show("输入的工号信息有误！");
			}
		}

		private void lbTeaCourse_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void ckbTeaCouFinish_CheckedChanged(object sender, EventArgs e)
		{
			
			string couName = "";
			try
			{
				couName = cbbTeaCourse.SelectedItem.ToString();
			}
			catch
			{
				MessageBox.Show("请选择课程！");
			}
			
			dgvSource.Visible = ckbTeaCouFinish.Checked;
			btnTeaCouSouComfrim.Enabled = ckbTeaCouFinish.Checked;


			string str;
			string[] arr;
			DataTable datatable = new DataTable();
			datatable.Columns.Add(new DataColumn("学号", typeof(int)));
			datatable.Columns.Add(new DataColumn("姓名", typeof(string)));
			datatable.Columns.Add(new DataColumn("成绩", typeof(int)));
			//读取成绩
			fsIn = new FileStream(path + "\\" + couName, FileMode.OpenOrCreate, FileAccess.Read);
			sr = new StreamReader(fsIn);
			//	Dictionary<int, int> courseDit = new Dictionary<int, int>();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				DataRow drow = datatable.NewRow();
				int s_id = int.Parse(arr[0]);
				drow[0] = s_id;
				drow[1] = xueDict[s_id].name;
				drow[2] = int.Parse(arr[1]);
				datatable.Rows.Add(drow);
			}
			dgvSource.DataSource = datatable;
			dgvSource.Columns[0].ReadOnly = true;
			dgvSource.Columns[1].ReadOnly = true;
			dgvSource.Columns[0].Width = dgvSource.Columns[1].Width = 60;
			fsIn.Close();
			sr.Close();
		}

		private void btnTeaCouSouComfrim_Click(object sender, EventArgs e)
		{
			string couName = cbbTeaCourse.SelectedItem.ToString();
			fsOut = new FileStream(path + "\\" + couName, FileMode.OpenOrCreate, FileAccess.Write);
			sw = new StreamWriter(fsOut);			
			
			DataTable datatable = dgvSource.DataSource as DataTable;
			for (int i = 0; i < datatable.Rows.Count; i++)
			{
				DataRow dr = datatable.Rows[i];
				sw.WriteLine(dr[0].ToString() + ' ' + dr[2].ToString());
			}
			sw.Flush();
			sw.Close();
			fsOut.Close();

			foreach(Kecheng cou in keList)
			{
				if(cou.name.Equals(couName))
				{
					cou.finish = 1;
					break;
				}
			}
			ckbTeaCouFinish.Checked = false;
			ckbTeaCouFinish.Enabled = false;
			btnTeaCouSouComfrim.Enabled = false;
			lblTeaCouIsFinish.Text = "是";
			MessageBox.Show("录入成绩完毕，该课程完结！");
		}

		private void dgvHomework_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			DataRow dr = (dgvHomework.DataSource as DataTable).Rows[dgvHomework.CurrentCell.RowIndex];
			string stuName = dr[0].ToString();

			foreach( Zuoye hw in hwList )
			{
				if( hw.stu.Equals(stuName) )
				{
					rtxtTeaHomework.Text = hw.hw;
					txtTeaHomeworkSource.Enabled = true;
					btnTeaHomeworkFinish.Enabled = true;
					break;
				}
			}
		}

		private void btnTeaHomeworkFinish_Click(object sender, EventArgs e)
		{
			DataRow dr = (dgvHomework.DataSource as DataTable).Rows[dgvHomework.CurrentCell.RowIndex];
			string stuName = dr[0].ToString();
			fsOut = new FileStream(path + "\\" + curKe.name + "_h", FileMode.OpenOrCreate, FileAccess.Write);
			sw = new StreamWriter(fsOut);
			foreach (Zuoye hw in hwList)
			{
				if (hw.stu.Equals(stuName))
				{
					hw.read = 1;
					hw.source = int.Parse(txtTeaHomeworkSource.Text);
					dr[1] = "已批阅";
					dr[2] = hw.source.ToString();
				}
				sw.WriteLine(hw.stu + ' ' + hw.hw + ' ' + hw.read.ToString() + ' ' + hw.source.ToString());
			}
			sw.Flush();
			sw.Close();
			fsOut.Close();
		}

		private void rbHomeworkOpen_CheckedChanged(object sender, EventArgs e)
		{
			foreach( Kecheng cou in keList )
			{
				if( cou == curKe )
				{
					cou.homework = 1;
					curKe = cou;
					break;
				}
			}
		}

		private void rbHomeworkClose_CheckedChanged(object sender, EventArgs e)
		{
			foreach (Kecheng cou in keList)
			{
				if (cou == curKe)
				{
					cou.homework = 0;
					curKe = cou;
					break;
				}
			}
		}

		private void btnStuHomekorkUp_Click(object sender, EventArgs e)
		{
			string couName = cbbStuCourse.SelectedItem.ToString();
			fsOut = new FileStream(path + "\\" + couName + "_h", FileMode.OpenOrCreate);
			fsOut.Close();
			fsOut = new FileStream(path + "\\" + couName + "_h", FileMode.Append);
			sw = new StreamWriter(fsOut);
			sw.WriteLine(curStu.name + ' ' + rtxtStuHomekorkAsw.Text + " 0 0");
			sw.Flush();
			sw.Close();
			fsOut.Close();
			MessageBox.Show("作业提交成功！");
		}

		private void btnStuAsk_Click(object sender, EventArgs e)
		{
			string couName = cbbStuCourse.SelectedItem.ToString();
			fsOut = new FileStream(path + "\\" + couName + "_a", FileMode.OpenOrCreate);
			fsOut.Close();
			fsOut = new FileStream(path + "\\" + couName + "_a", FileMode.Append);
			sw = new StreamWriter(fsOut);
			sw.WriteLine(curStu.name + ' ' + rtxtStuAsk.Text + " 0 0");
			sw.Flush();
			sw.Close();
			fsOut.Close();
			MessageBox.Show("提问提交成功！");
		}

		private void lbAskStu_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(lbAskStu.SelectedItem == null)
			{
				return;
			}
			string stuName = lbAskStu.SelectedItem.ToString();

			foreach( Dayi ans in ansList )
			{
				if( ans.stu.Equals(stuName) )
				{
					rtxtTeaAsk.Text = ans.ask;
					rtxtTeaAnswer.Enabled = true;
					btnTeaAnswer.Enabled = true;
					break;
				}
			}
		}

		private void btnTeaAnswer_Click(object sender, EventArgs e)
		{
			string stuName = "";
			try
			{
				stuName = lbAskStu.SelectedItem.ToString();
			}
			catch
			{
				MessageBox.Show("请选择学生！");
				return;
			}

			if(rtxtTeaAnswer.Text == "")
			{
				MessageBox.Show("请输入解答！");
				return;
			}

			
			string couName = cbbTeaCourse.SelectedItem.ToString();
			fsOut = new FileStream(path + "\\" + couName + "_a", FileMode.OpenOrCreate, FileAccess.Write);
			sw = new StreamWriter(fsOut);
			foreach (Dayi ans in ansList)
			{
				if( ans.stu.Equals(stuName) )
				{
					ans.ans = rtxtTeaAnswer.Text;
					ans.read = 1;
				}
				sw.WriteLine(ans.stu + ' ' + ans.ask + ' ' + ans.read + ' ' + ans.ans);
			}
			lbAskStu.Items.Remove(stuName);
			sw.Flush();
			sw.Close();
			fsOut.Close();
			rtxtTeaAsk.Clear();
			rtxtTeaAnswer.Clear();
			MessageBox.Show("解答成功！");
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			string mailSend1 = textBox3.Text;
            string mailSend2 = textBox4.Text;
            string mailSend3 = textBox5.Text;
            string mailTitle = textBox1.Text;
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

           

            msg.To.Add(mailSend1);
            msg.To.Add(mailSend2);
            msg.To.Add(mailSend3);

            msg.From = new MailAddress(txtSendMail.Text, "上大通信2016", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/

            msg.Subject = mailTitle;  //邮件标题  

            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码  

            msg.Body = rtxtMail.Text;//邮件内容  
			msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码  
			msg.IsBodyHtml = false;//是否是HTML邮件  
			msg.Priority = MailPriority.High;//邮件优先级 

			SmtpClient client = new SmtpClient();

            client.Credentials = new System.Net.NetworkCredential(txtSendMail.Text, textBox2.Text);
            client.Port = 25;//使用的端口    
           
            client.EnableSsl = true;//经过ssl加密
            client.Host = "smtp.163.com";
           
            object userState = msg;
			try
			{
				client.SendAsync(msg, userState);
				//简单一点儿可以client.Send(msg);  
				MessageBox.Show("发送成功");
			}
			catch (System.Net.Mail.SmtpException ex)
			{
				MessageBox.Show(ex.Message, "发送邮件出错");
			}
		}
		
		private void cbbStuCourse_SelectedIndexChanged(object sender, EventArgs e)
		{
			string couName = cbbStuCourse.SelectedItem.ToString();
			foreach (Kecheng cou in keList)
			{
				if (cou.name.Equals(couName))
				{
					rtxtStuAsk.Clear();
					rtxtStuAnswer.Clear();
					if (cou.finish == 1)
					{
						lblStuCouFinish.Text = "是";
						lblStuCouSource.Text = xueKeFenDict[couName].ToString();
						btnStuCancellCourse.Enabled = false;
						btnStuHomekorkUp.Enabled = false;
						rtxtStuHomekorkAsw.Enabled = false;
						rtxtStuAsk.Enabled = false;
						btnStuAsk.Enabled = false;
					}
					else
					{
						lblStuCouFinish.Text = "否";
						lblStuCouSource.Text = "";
						btnStuCancellCourse.Enabled = true;
						rtxtStuAsk.Enabled = true;
						btnStuAsk.Enabled = true;
						if (cou.homework == 1)
						{
							lblStuIsHomework.Text = "有待提交作业";
							btnStuHomekorkUp.Enabled = true;
							rtxtStuHomekorkAsw.Enabled = true;
						}
						else
						{
							lblStuIsHomework.Text = "无待提交作业";
							btnStuHomekorkUp.Enabled = false;
							rtxtStuHomekorkAsw.Enabled = false;
						}
					}
					break;
				}
			}
			string str;
			string[] arr;
			fsIn = new FileStream(path + "\\" + couName + "_a", FileMode.OpenOrCreate, FileAccess.Read);
			sr = new StreamReader(fsIn);
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				if (curStu.name.Equals(arr[0]))
				{
					if (int.Parse(arr[2]) == 1)
					{
						rtxtStuAsk.Text = arr[1];
						rtxtStuAnswer.Text = arr[3];
					}
				}
			}
			fsIn.Close();
			sr.Close();
		}

		private void cbbTeaCourse_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbbTeaCourse.SelectedItem == null)
			{
				return;
			}
			string couName = cbbTeaCourse.SelectedItem.ToString();
			string str;
			string[] arr;
			foreach (Kecheng cou in keList)
			{
				if (cou.name.Equals(couName))
				{
					curKe = cou;
					if (cou.finish == 1)
					{
						lblTeaCouIsFinish.Text = "是";
						ckbTeaCouFinish.Enabled = false;
						ckbTeaCouFinish.Checked = false;
						btnTeaCouSouComfrim.Enabled = false;
						
						dgvHomework.DataSource = new DataTable();
						rtxtTeaHomework.Clear();
						txtTeaHomeworkSource.Clear();
					}
					else
					{
						lblTeaCouIsFinish.Text = "否";
						ckbTeaCouFinish.Enabled = true;
						ckbTeaCouFinish.Checked = false;
						btnTeaCouSouComfrim.Enabled = false;
						rbHomeworkClose.Enabled = rbHomeworkOpen.Enabled = true;
						txtTeaHomeworkSource.Enabled = false;
						btnTeaHomeworkFinish.Enabled = false;
						lbAskStu.Enabled = true;
						rtxtTeaAnswer.Enabled = false;
						btnTeaAnswer.Enabled = false;

						//读取作业信息
						fsIn = new FileStream(path + "\\" + couName + "_h", FileMode.OpenOrCreate, FileAccess.Read);
						sr = new StreamReader(fsIn);
						hwList = new List<Zuoye>();
						DataTable dtable = new DataTable();
						dtable.Columns.Add(new DataColumn("姓名", typeof(string)));
						dtable.Columns.Add(new DataColumn("批阅", typeof(string)));
						dtable.Columns.Add(new DataColumn("分数", typeof(int)));

						while (sr.Peek() >= 0)
						{
							str = sr.ReadLine();
							arr = str.Split(' ');

							hwList.Add(new Zuoye(arr[0], arr[1], int.Parse(arr[2]), int.Parse(arr[3])));
							DataRow dr = dtable.NewRow();
							dr[0] = arr[0];
							dr[1] = (int.Parse(arr[2]) == 1) ? "已批阅" : "未批阅";
							dr[2] = int.Parse(arr[3]);
							dtable.Rows.Add(dr);
						}
						dgvHomework.DataSource = dtable;
						dgvHomework.ReadOnly = true;
						dgvHomework.Columns[0].Width = dgvHomework.Columns[1].Width = 70;
						dgvHomework.Columns[2].Width = 60;
						fsIn.Close();
						sr.Close();
					}
					if (cou.homework == 1)
					{
						rbHomeworkOpen.Checked = true;
					}
					else
					{
						rbHomeworkClose.Checked = true;
					}
					break;
				}
			}
			fsIn = new FileStream(path + "\\" + couName + "_a", FileMode.OpenOrCreate, FileAccess.Read);
			sr = new StreamReader(fsIn);
			ansList = new List<Dayi>();
			lbAskStu.Items.Clear();
			while (sr.Peek() >= 0)
			{
				str = sr.ReadLine();
				arr = str.Split(' ');
				ansList.Add(new Dayi(arr[0], arr[1], int.Parse(arr[2]), arr[3]));
				if (int.Parse(arr[2]) == 0)
				{
					lbAskStu.Items.Add(arr[0]);
				}
			}
			fsIn.Close();
			sr.Close();
		}

		private void lvManage_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void lbManage_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (lbManage.SelectedIndex)
			{
				case 0://学生信息
					dt = new DataTable();
					dt.Columns.Add(new DataColumn("学号", typeof(int)));
					dt.Columns.Add(new DataColumn("姓名", typeof(string)));
					dt.Columns.Add(new DataColumn("性别", typeof(string)));
					dt.Columns.Add(new DataColumn("年龄", typeof(int)));
					dt.Columns.Add(new DataColumn("年级", typeof(int)));
					dt.Columns.Add(new DataColumn("院系", typeof(string)));
					dt.Columns.Add(new DataColumn("专业", typeof(string)));
					dt.Columns.Add(new DataColumn("班级", typeof(int)));
					dt.Columns.Add(new DataColumn("绩点", typeof(float)));
					dt.Columns.Add(new DataColumn("本月出勤天数", typeof(int)));
					dt.Columns.Add(new DataColumn("竞赛获奖", typeof(string)));
					dt.Columns.Add(new DataColumn("学术交流次数", typeof(int)));
					for (int i = 0; i < xueList.Count; i++)
					{
						DataRow dr = dt.NewRow();
						dr[0] = xueList[i].id;
						dr[1] = xueList[i].name;
						dr[2] = xueList[i].sex;
						dr[3] = xueList[i].age;
						dr[4] = xueList[i].grade;
						dr[5] = xueList[i].faculty;
						dr[6] = xueList[i].major;
						dr[7] = xueList[i].classes;
						dr[8] = xueList[i].GPA;
						dr[9] = xueList[i].attendance;
						dr[10] = xueList[i].prize;
						dr[11] = xueList[i].exchange;
						dt.Rows.Add(dr);
					}
					dgv.DataSource = dt;
					dgv.ReadOnly = true;
					dgv.Columns[2].Width = dgv.Columns[3].Width = dgv.Columns[4].Width = dgv.Columns[7].Width = dgv.Columns[8].Width = 65;
					dgv.Columns[0].Width = dgv.Columns[1].Width = dgv.Columns[9].Width = 90;
					dgv.Columns[5].Width = 200;
					dgv.Columns[10].Width = 250;

					gpbTeacher.Enabled = false;
					gpbCourse.Enabled = false;
					gpbStudent.Enabled = true;

					rbStuLook.Checked = true;
					btnStudentUpdate.Enabled = false;
					cbbStuSeek.Enabled = false;
					txtStuSeek.Enabled = false;
					btnStuSeek.Enabled = false;
					break;
				case 1://老师信息
					dt = new DataTable();
					dt.Columns.Add(new DataColumn("工号", typeof(int)));
					dt.Columns.Add(new DataColumn("姓名", typeof(string)));
					dt.Columns.Add(new DataColumn("性别", typeof(string)));
					dt.Columns.Add(new DataColumn("年龄", typeof(int)));
					dt.Columns.Add(new DataColumn("院系", typeof(string)));
					dt.Columns.Add(new DataColumn("专业", typeof(string)));
					dt.Columns.Add(new DataColumn("职称", typeof(string)));
					dt.Columns.Add(new DataColumn("评教分数", typeof(float)));
					for (int i = 0; i < laoList.Count; i++)
					{
						DataRow dr = dt.NewRow();
						dr[0] = laoList[i].id;
						dr[1] = laoList[i].name;
						dr[2] = laoList[i].sex;
						dr[3] = laoList[i].age;
						dr[4] = laoList[i].faculty;
						dr[5] = laoList[i].major;
						dr[6] = laoList[i].title;
						dr[7] = laoList[i].appraise;
						dt.Rows.Add(dr);
					}
					dgv.DataSource = dt;
					dgv.ReadOnly = true;
					dgv.Columns[0].Width = dgv.Columns[1].Width = dgv.Columns[5].Width = dgv.Columns[6].Width = dgv.Columns[7].Width = 110;
					dgv.Columns[4].Width = 200;

					gpbStudent.Enabled = false;
					gpbCourse.Enabled = false;
					gpbTeacher.Enabled = true;
					rbTeaLook.Checked = true;
					btnTeaUpdate.Enabled = false;
					cbbTeaSeek.Enabled = false;
					txtTeaSeek.Enabled = false;
					btnTeaSeek.Enabled = false;
					break;
				case 2://课程信息
					dt = new DataTable();
					dt.Columns.Add(new DataColumn("课程号", typeof(int)));
					dt.Columns.Add(new DataColumn("课程名", typeof(string)));
					dt.Columns.Add(new DataColumn("教师1", typeof(string)));
					dt.Columns.Add(new DataColumn("教师2", typeof(string)));
					dt.Columns.Add(new DataColumn("教师3", typeof(string)));
					dt.Columns.Add(new DataColumn("是否结课", typeof(string)));
					for (int i = 0; i < keList.Count; i++)
					{
						DataRow dr = dt.NewRow();
						dr[0] = keList[i].id;
						dr[1] = keList[i].name;
						dr[2] = laoDict[keList[i].teacher1].name;
						dr[3] = laoDict[keList[i].teacher2].name;
						dr[4] = laoDict[keList[i].teacher3].name;
						dr[5] = (keList[i].finish == 1) ? "是" : "否";
						dt.Rows.Add(dr);
					}
					dgv.DataSource = dt;
					dgv.ReadOnly = true;
					dgv.Columns[0].Width = dgv.Columns[2].Width = dgv.Columns[3].Width = dgv.Columns[4].Width = dgv.Columns[5].Width = 140;
					dgv.Columns[1].Width = 220;

					gpbTeacher.Enabled = false;
					gpbStudent.Enabled = false;
					gpbCourse.Enabled = true;
					rbCourseLook.Checked = true;
					btnCourseUpdata.Enabled = false;
					cbbCourseSeek.Enabled = false;
					txtCourseSeek.Enabled = false;
					btnCourseSeek.Enabled = false;
					btnCourseSource.Enabled = true;
					btnCourseBack.Enabled = false;
					break;
				default:
					break;
			}
		}

		private void rbStuUpdate_CheckedChanged(object sender, EventArgs e)
		{
			dgv.ReadOnly = !rbStuUpdate.Checked;
			btnStudentUpdate.Enabled = rbStuUpdate.Checked;
		}

		private void rbStuSeek_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStuSeek.Checked)
			{
				//先对数据进行更新
				dt = dgv.DataSource as DataTable;
				xueDict.Clear();
				xueList.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dr = dt.Rows[i];
					int id = int.Parse(dr[0].ToString());
					string name = dr[1].ToString();
					string sex = dr[2].ToString();
					int age = int.Parse(dr[3].ToString());
					int grade = int.Parse(dr[4].ToString());
					string faculty = dr[5].ToString();
					string major = dr[6].ToString();
					int classes = int.Parse(dr[7].ToString());
					float GPA = float.Parse(dr[8].ToString());
					int attendance = int.Parse(dr[9].ToString());
					string prize = dr[10].ToString();
					int exchange = int.Parse(dr[11].ToString());
					xueDict[id] = new Xuesheng(id, name, sex, age, grade, faculty, major, classes, GPA, attendance, prize, exchange);
					xueList.Add(xueDict[id]);
				}
				dgv.ReadOnly = true;
				cbbStuSeek.Enabled = true;
				txtStuSeek.Enabled = true;
				btnStuSeek.Enabled = true;
				cbbStuSeek.SelectedIndex = 0;
			}
			else
			{
				dgv.DataSource = dt;

				cbbStuSeek.Enabled = false;
				txtStuSeek.Enabled = false;
				btnStuSeek.Enabled = false;
			}

		}

		private void rbTeaUpdate_CheckedChanged(object sender, EventArgs e)
		{
			dgv.ReadOnly = !rbTeaUpdate.Checked;
			btnTeaUpdate.Enabled = rbTeaUpdate.Checked;
		}

		private void rbTeaSeek_CheckedChanged(object sender, EventArgs e)
		{
			if (rbTeaSeek.Checked)
			{
				//先对数据进行更新
				dt = dgv.DataSource as DataTable;
				laoDict.Clear();
				laoList.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dr = dt.Rows[i];
					int id = int.Parse(dr[0].ToString());
					string name = dr[1].ToString();
					string sex = dr[2].ToString();
					int age = int.Parse(dr[3].ToString());
					string faculty = dr[4].ToString();
					string major = dr[5].ToString();
					string title = dr[6].ToString();
					float appraise = float.Parse(dr[7].ToString());
					laoDict[id] = new Laoshi(id, name, sex, age, faculty, major, title, appraise);
					laoList.Add(laoDict[id]);
				}
				dgv.ReadOnly = true;
				cbbTeaSeek.Enabled = true;
				txtTeaSeek.Enabled = true;
				btnTeaSeek.Enabled = true;
				cbbTeaSeek.SelectedIndex = 0;
			}
			else
			{
				dgv.DataSource = dt;

				cbbTeaSeek.Enabled = false;
				txtTeaSeek.Enabled = false;
				btnTeaSeek.Enabled = false;
			}
		}

		private void rbCourseUpdate_CheckedChanged(object sender, EventArgs e)
		{
			dgv.ReadOnly = !rbCourseUpdate.Checked;
			btnCourseUpdata.Enabled = rbCourseUpdate.Checked;
		}

		private void rbCourseSeek_CheckedChanged(object sender, EventArgs e)
		{
			if (rbCourseSeek.Checked)
			{
				//先对数据进行更新
				dt = dgv.DataSource as DataTable;
				List<Kecheng> tempList = new List<Kecheng>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dr = dt.Rows[i];
					int id = int.Parse(dr[0].ToString());
					string name = dr[1].ToString();
					string tea1 = dr[2].ToString();
					string tea2 = dr[3].ToString();
					string tea3 = dr[4].ToString();
					int t1 = -1, t2 = -1, t3 = -1;
					foreach (Laoshi tr in laoList)
					{
						if (tr.name.Equals(tea1))
						{
							t1 = tr.id;
						}
						else if (tr.name.Equals(tea2))
						{
							t2 = tr.id;
						}
						else if (tr.name.Equals(tea3))
						{
							t3 = tr.id;
						}
					}
					if (t1 == -1)
					{
						MessageBox.Show("教师“" + tea1 + "”不存在，本次更新失败，请重新确认！");
						rbCourseSeek.Checked = true;
						return;
					}
					if (t2 == -1)
					{
						MessageBox.Show("教师“" + tea2 + "”不存在，本次更新失败，请重新确认！");
						rbCourseSeek.Checked = true;
						return;
					}
					if (t2 == -1)
					{
						MessageBox.Show("教师“" + tea2 + "”不存在，本次更新失败，请重新确认！");
						rbCourseSeek.Checked = true;
						return;
					}
					string finish = dr[5].ToString();
					tempList.Add(new Kecheng(id, name, t1, t2, t3, finish.Equals("是") ? 1 : 0));
				}
				keList = tempList;
				
				dgv.ReadOnly = true;
				cbbCourseSeek.Enabled = true;
				txtCourseSeek.Enabled = true;
				btnCourseSeek.Enabled = true;
				cbbCourseSeek.SelectedIndex = 0;
			}
			else
			{
				dgv.DataSource = dt;

				cbbCourseSeek.Enabled = false;
				txtCourseSeek.Enabled = false;
				btnCourseSeek.Enabled = false;
			}
		}

		private void rbCourseLook_CheckedChanged(object sender, EventArgs e)
		{
			btnCourseSource.Enabled = rbCourseLook.Checked;
		}

		private void label25_Click(object sender, EventArgs e)
		{

		}

		private void lblTeaName_Click(object sender, EventArgs e)
		{

		}

		private void cbbSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbbSelect.SelectedIndex == 0)
			{
				gpbManage.Size = new Size(980, 592);
				gpbStu.Size = new Size(0, 0);
				gpbTea.Size = new Size(0, 0);
				gpbNotice.Size = new Size(0, 0);
				gpbManage.Visible = true;
				gpbStu.Visible = false;
				gpbTea.Visible = false;
				gpbNotice.Visible = false;

				gpbStudent.Enabled = false;
				gpbTeacher.Enabled = false;
				gpbCourse.Enabled = false;
			}
			else if (cbbSelect.SelectedIndex == 1)
			{
				gpbManage.Size = new Size(0, 0);
				gpbStu.Size = new Size(980, 592);
				gpbTea.Size = new Size(0, 0);
				gpbNotice.Size = new Size(0, 0);
				gpbManage.Visible = false;
				gpbStu.Visible = true;
				gpbTea.Visible = false;
				gpbNotice.Visible = false;

				txtStuLogin.Clear();
				lblStuID.Text = "";
				lblStuName.Text = "";
				lblStuGrade.Text = "";
				lblStuFaculty.Text = "";
				lblStuMajor.Text = "";
				lblStuClass.Text = "";
				lblStuGPA.Text = "";
				cbbStuCourse.Items.Clear();
				cbbStuSelectCourse.Items.Clear();
				lblStuCouFinish.Text = "";
				lblStuCouSource.Text = "";
				lblStuIsHomework.Text = "";
				rtxtStuHomekorkAsw.Clear();
				gpbStuCourse.Enabled = false;
				ckbStuSelectCourse.Checked = false;
				cbbStuSelectCourse.Visible = false;
				btnStuSelectCourseComfrim.Visible = false;
				rtxtStuAsk.Clear();
				rtxtStuAnswer.Clear();
			}
			else if (cbbSelect.SelectedIndex == 2)
			{
				gpbManage.Size = new Size(0, 0);
				gpbStu.Size = new Size(0, 0);
				gpbTea.Size = new Size(980, 592);
				gpbNotice.Size = new Size(0, 0);
				gpbManage.Visible = false;
				gpbStu.Visible = false;
				gpbTea.Visible = true;
				gpbNotice.Visible = false;

				txtTeaLogin.Clear();
				lblTeaID.Text = "";
				lblTeaName.Text = "";
				lblTeaTitle.Text = "";
				lblTeaFaculty.Text = "";
				lblTeaMajor.Text = "";
				lblTeaAppraise.Text = "";
				cbbTeaCourse.Items.Clear();
				lblTeaCouIsFinish.Text = "";
				ckbTeaCouFinish.Enabled = false;
				btnTeaCouSouComfrim.Enabled = false;
				dgvSource.Visible = false;
				gpbTeaCourse.Enabled = false;
				dgvHomework.DataSource = new DataTable();
				rtxtTeaHomework.Clear();
				txtTeaHomeworkSource.Clear();
			}
			else
			{
				gpbManage.Size = new Size(0, 0);
				gpbStu.Size = new Size(0, 0);
				gpbTea.Size = new Size(0, 0);
				gpbNotice.Size = new Size(980, 592);
				gpbManage.Visible = false;
				gpbStu.Visible = false;
				gpbTea.Visible = false;
				gpbNotice.Visible = true;
			}
		}

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtxtMail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSendMail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void gpbNotice_Enter(object sender, EventArgs e)
        {

        }

        private void txtStuLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbbStuSelectCourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rtxtTeaHomework_TextChanged(object sender, EventArgs e)
        {

        }

        private void gpbTeaCourse_Enter(object sender, EventArgs e)
        {

        }

        private void rtxtStuHomekorkAsw_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblStuIsHomework_Click(object sender, EventArgs e)
        {

        }

        private void lblStuCouSource_Click(object sender, EventArgs e)
        {

        }

        private void lblStuCouFinish_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void rtxtStuAsk_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
	}

	public class Xuesheng
	{
		public int id;
		public string name;
		public string sex;
		public int age;
		public int grade;
		public string faculty;
		public string major;
		public int classes;
		public float GPA;
		public int attendance;
		public string prize;
		public int exchange;

		public Xuesheng(int i, string n, string s, int a, int g, string f, string m, int c, float G, int at, string pr, int e)
		{
			id = i;
			name = n;
			sex = s;
			age = a;
			grade = g;
			faculty = f;
			major = m;
			classes = c;
			GPA = G;
			attendance = at;
			prize = pr;
			exchange = e;
		}
	}

	public class Laoshi
	{
		public int id;
		public string name;
		public string sex;
		public int age;
		public string faculty;
		public string major;
		public string title;
		public float appraise;
		
		public Laoshi(int i, string n, string s, int a, string f, string m, string t, float p)
		{
			id = i;
			name = n;
			sex = s;
			age = a;
			faculty = f;
			major = m;
			title = t;
			appraise = p;
		}
	}

	public class Kecheng
	{
		public int id;
		public string name;
		public int teacher1;
		public int teacher2;
		public int teacher3;
		public int finish;
		public int homework;

		public Kecheng(int i, string n, int t1, int t2, int t3, int f)
		{
			id = i;
			name = n;
			teacher1 = t1;
			teacher2 = t2;
			teacher3 = t3;
			finish = f;
			homework = 0;
		}
	}

	public class Zuoye
	{
		public string stu;
		public string hw;
		public int read;
		public int source;

		public Zuoye(string s, string h, int r, int c)
		{
			stu = s;
			hw = h;
			read = r;
			source = c;
		}
	}

	public class Dayi
	{
		public string stu;
		public string ask;
		public int read;
		public string ans;

		public Dayi(string s, string ak, int r, string an)
		{
			stu = s;
			ask = ak;
			read = r;
			ans = an;
		}
	}
}
