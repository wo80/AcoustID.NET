// -----------------------------------------------------------------------
// <copyright file="Image.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Chromaprint
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Image
    {
        private const int BUFFER_BLOCK_SIZE = 2048;

        private int m_rows;
        private int m_columns;
        private double[] m_data;

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int Columns
        {
            get { return m_columns; }
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int Rows
        {
            get { return m_rows; }
        }

        public double this[int i, int j]
        {
            get { return m_data[m_columns * i + j]; }
            set { m_data[m_columns * i + j] = value; }
        }

        internal double[] Data
        {
            get { return m_data; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        /// <param name="columns">The number of columns.</param>
        public Image(int columns)
            : this(columns, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        /// <param name="columns">The number of columns.</param>
        /// <param name="rows">The number of rows.</param>
        public Image(int columns, int rows)
        {
            m_rows = rows;
            m_columns = columns;
            m_data = new double[Math.Max(columns * rows, BUFFER_BLOCK_SIZE)];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        /// <param name="columns">The number of columns.</param>
        /// <param name="data">The image data.</param>
        public Image(int columns, double[] data)
        {
            m_rows = data.Length / columns;
            m_columns = columns;
            m_data = data;
        }

        internal double Get(int i, int j)
        {
            return m_data[m_columns * i + j];
        }

        internal void Set(int i, int j, double value)
        {
            m_data[m_columns * i + j] = value;
        }

        internal void AddRow(double[] row)
        {
            int n = m_rows * m_columns;

            int size = m_data.Length;

            if (n + m_columns > size)
            {
                Array.Resize(ref m_data, size + BUFFER_BLOCK_SIZE);
            }

            for (int i = 0; i < m_columns; i++)
            {
                m_data[n + i] = row[i];
            }

            m_rows++;
        }

        internal double[] Row(int i)
        {
            //assert(0 <= i && i < NumRows());

            double[] row = new double[m_columns];
            int n = i * m_columns;

            for (int j = 0; j < m_columns; j++)
            {
                row[j] = m_data[n + j];
            }

            return row;
        }
    }
}
