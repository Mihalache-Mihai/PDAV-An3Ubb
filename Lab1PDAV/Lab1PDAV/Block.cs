using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1PDAV
{
    class Block
    {
        int size;
        public double[,] matrix;
       public  int indexi;
       public  int indexj;

        public Block(int size,int indexi,int indexj)
        {
            this.indexi = indexi;
            this.indexj = indexj;
            this.size = size;
            matrix = new double[size, size];  
        }
        public Block(int size)
        {
            this.size = size;
            matrix = new double[size, size];
        }

    }
}
