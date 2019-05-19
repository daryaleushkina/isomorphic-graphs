using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace CourseWork
{
    public partial class MainWindow : Form
    {
        private GViewer gViewer;
        bool[] visited = new bool[100];
        bool[] used = new bool[100];
        int n, m;
        private Graph graph = new Graph();
        private Data data;
        
        private List<int[,]> subTree = new List<int[,]>();
        private List<string> maxMatrixCode = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            gViewer = new GViewer()
            {
                Dock = DockStyle.Fill
            };
            SuspendLayout();
            panel.Controls.Add(gViewer);
            ResumeLayout();
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Data temp = new Data(new StreamReader(openFileDialog.FileName));//выбрали файл
                subTree = temp.GetMatrix;//получили матрицу поддеревьев
                subTree = nonIsomorphicTrees(subTree);//получили неизоморфные поддеревья
                mergeMatrix(subTree);//объединили всё в одну матрицу и в один файл
                data = new Data(new StreamReader("C:/Users/Полина/Desktop/Курсач/CourseWork/DoNotTouch.txt"));//считали этот самый файл
                for (int i = 0; i < data.GetTreeData.Count; i++) 
                if (!used[i])
                    dfs(i);
                ShowInfoAboutGraph();
            }
        }

        //для корректной отрисовки объединяем все матрицы в одну и записываем их в файл. 
        private void mergeMatrix(List<int[,]> subTree)
        {
            int n = subTree.Count;
            int m = Convert.ToInt32(Math.Sqrt(subTree[0].Length));
            int[,] newMatrix = new int[n*m, n*m];
            for (int count = 0; count < subTree.Count; count++)
            {
                for (int i = m * count; i < m * (count + 1); i++)
                    for (int j = m * count; j < m * (count + 1); j++)
                        newMatrix[i, j] = subTree[count][i % m, j % m];
            }
            System.IO.File.Delete("C:/Users/Полина/Desktop/Курсач/CourseWork/DoNotTouch.txt");
            for (int i = 0; i < n * m; i++)
            {
                for (int j = 0; j < n * m; j++)
                    if (j < n * m - 1)
                        System.IO.File.AppendAllText("C:/Users/Полина/Desktop/Курсач/CourseWork/DoNotTouch.txt", newMatrix[i, j].ToString() + ' ');
                    else
                        System.IO.File.AppendAllText("C:/Users/Полина/Desktop/Курсач/CourseWork/DoNotTouch.txt", newMatrix[i, j].ToString());
                System.IO.File.AppendAllText("C:/Users/Полина/Desktop/Курсач/CourseWork/DoNotTouch.txt", "\r\n".ToString());
            }
        }
        private void dfs(int v)
        {
            used[v] = true;
            for (int i = data.GetTreeData[v].Childs.Count -1 ; i >= 0; i--)
            {
                int to = data.GetTreeData[v].Childs[i];
                if (!used[to])
                {
                    graph.AddEdge((data.GetTreeData[v].ID + 1).ToString(),
                      (data.GetTreeData[v].Childs[i] + 1).ToString()).Attr.ArrowheadAtTarget = ArrowStyle.None;
                    m++;
                    dfs(to);
                }
            }
        }
        private int comp(int v)
        {
            int visitedVertices = 1;
            visited[v] = true;               // помечаем вершину как пройденную
            for (int i = 0; i < data.GetTreeData[v].Childs.Count; i++) 
            { 
                int to = data.GetTreeData[v].Childs[i];                      // проходим по смежным с v вершинам
                if (!visited[to])                    // проверяем, не находились ли мы ранее в выбранной вершине
                {
                    visitedVertices += comp(to);
                    Console.WriteLine(visitedVertices);
                }
            }
            return visitedVertices;
        }
        private int getEdge()//возвращает количество ребер
        {
            int edge = 0;
            for (int i = 0; i < data.GetTreeData.Count; i++)
                for (int j = 0; j < data.GetTreeData[i].Childs.Count; j++)
                    if (data.GetTreeData[i].Childs[j] > i)
                        edge++;
            return edge;
        }
        //возвращает список поддеревьев не изоморфных друг другу
        private List<int[,]> nonIsomorphicTrees(List<int[,]> subTree)
        {
            int i = 0;
            while (i < subTree.Count - 1) 
            {
                MaxMatrixCode temp1 = new MaxMatrixCode(subTree[i]);
                int j = i + 1;
                while (j < subTree.Count) 
                {
                    
                    MaxMatrixCode temp2 = new MaxMatrixCode(subTree[j]);
                    if (temp1.GetMaxMatrixCode == temp2.GetMaxMatrixCode)//сравниваем максимальный матричный код. если равен - удаляем
                    {
                        subTree.Remove(subTree[j]);
                        j--;
                    }
                    j++;
                }
                i++;
            }

            for (int j = 0; j < subTree.Count; j++)
            {
                MaxMatrixCode temp1 = new MaxMatrixCode(subTree[j]);
                maxMatrixCode.Add(temp1.GetMaxMatrixCode.ToString());
            }

            return subTree;
        }
        //проверка
        private bool check()
        {
            n = data.GetTreeData.Count;
            m = getEdge();
            if (comp(0) == n && n == m + 1) 
                return true;

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show("Граф не является деревом", "Некорректные данные", buttons);

            return false;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void ShowInfoAboutGraph()
        {
            //проходимся по нашим вершинам и убираем подпись
            foreach (Node item in graph.Nodes)
            {
                item.LabelText = "";
                item.Attr.XRadius = 15;
                item.Attr.YRadius = 15;
            }
            gViewer.Graph = graph;
            //this.listMaxCode.Items.Clear();
            //this.listMaxCode.Text = "";
            foreach (string item in maxMatrixCode)
            {
                this.listMaxCode.Items.Add(item);
            }
        }
    }
}