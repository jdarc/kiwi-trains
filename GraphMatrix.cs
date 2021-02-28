using System;

namespace KiwiTrains
{
    internal class GraphMatrix
    {
        private readonly int[] _elements;

        public int Size => (int) Math.Sqrt(_elements.Length);

        public GraphMatrix(int size) => _elements = new int[size * size];

        public void SetElementAt(int row, int col, int value) => _elements[row * Size + col] = value;

        public int GetElementAt(int row, int col) => _elements[row * Size + col];
    }
}