using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CourseWork
{
  
    struct NodeStruct
    {
        public int ID;
        public List<int> Childs;
        public NodeStruct(int ID, List<int> Childs)
        {
            this.ID = ID;
            this.Childs = new List<int>();
            if (Childs != null)
                for (int i = 0; i < Childs.Count; i++)
                    this.Childs.Add(Childs[i]);
        }
    }

    class Data
    {
       
        private List<NodeStruct> treeData;
        private int countIsolated = 0;
        private List<int> nodeIsolated = new List<int>();
        private int[,] graphMatrix;

        public Data(StreamReader stream)
        {
           //строим структуру дерева и паралельно делаем матрицу самого нашего входного файла.
            treeData = new List<NodeStruct>();

            try
            {
                using (stream)
                {
                    string line;
                    int c = 0;
                    treeData.Clear();
      
                    while ((line = stream.ReadLine()) != null)
                    {
                        string[] lineParams = line.Split(' ');
                        List<int> childs = new List<int>();
                        if (c == 0)
                        {
                            int n = lineParams.Count();
                            graphMatrix = new int[n, n];
                        }
                        for (int i = 0; i < lineParams.Count(); i++)
                        {
                            if (lineParams[i] == "1" && i != c)
                            {
                                childs.Add(i);
                                graphMatrix[c, i] = 1;
                            }
                            else
                            {
                                graphMatrix[c, i] = 0;
                            }
                        }
                        if (childs.Count == 1)
                        {
                            countIsolated++;
                            nodeIsolated.Add(c);
                        }
                        NodeStruct treeBranch = new NodeStruct(Convert.ToInt32(c), childs);
                        treeData.Add(treeBranch);
                        c++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //получаем саму структуру дерева, с которой в дальнейшем работаем
        public List<NodeStruct> GetTreeData
        {
            get { return treeData; }
        }
        //получаем количество листьев
        public int GetCountIsolated
        {
            get { return countIsolated; }
        }
        public List<int> GetNodeIsolated
        {
            get { return nodeIsolated; }
        }
        //получаем матрицы поддеревьев. Кладём их в list subTree
        public List<int[,]> GetMatrix
        {
            get
            {
                List<int[,]> subTree = new List<int[,]>();
                if (countIsolated > 0)
                {
                    int n = treeData.Count;
                    
                    for (int count = 0; count < GetCountIsolated; count++)
                    {
                        int[,] array = new int[graphMatrix.GetLength(0) - 1, graphMatrix.GetLength(1) - 1];

                        for (int i = 0, j = 0; i < graphMatrix.GetLength(0); i++)
                        {
                            if (i == GetNodeIsolated[count])
                                continue;

                            for (int k = 0, u = 0; k < graphMatrix.GetLength(1); k++)
                            {
                                if (k == GetNodeIsolated[count])
                                    continue;

                                array[j, u] = graphMatrix[i, k];
                                u++;
                            }
                            j++;
                        }

                        subTree.Add(array);
                    }
                }
                return subTree;
            }
        }
    }
}
