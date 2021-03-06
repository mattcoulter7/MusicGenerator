// <copyright file="CudaLinearAlgebraProvider.Double.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2015 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Security;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Providers.LinearAlgebra;
using Complex = System.Numerics.Complex;

namespace MathNet.Numerics.Providers.CUDA.LinearAlgebra
{
    /// <summary>
    /// NVidia's CUDA Toolkit linear algebra provider.
    /// </summary>
    internal partial class CudaLinearAlgebraProvider
    {
        /// <summary>
        /// Computes the requested <see cref="Norm"/> of the matrix.
        /// </summary>
        /// <param name="norm">The type of norm to compute.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="matrix">The matrix to compute the norm from.</param>
        /// <returns>
        /// The requested <see cref="Norm"/> of the matrix.
        /// </returns>
        public double MatrixNorm(Norm norm, int rows, int columns, double[] matrix)
        {
            return ManagedLinearAlgebraProvider.Instance.MatrixNorm(norm, rows, columns, matrix);
        }

        /// <summary>
        /// Computes the dot product of x and y.
        /// </summary>
        /// <param name="x">The vector x.</param>
        /// <param name="y">The vector y.</param>
        /// <returns>The dot product of x and y.</returns>
        /// <remarks>This is equivalent to the DOT BLAS routine.</remarks>
        [SecuritySafeCritical]
        public double DotProduct(double[] x, double[] y)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (x.Length != y.Length)
            {
                throw new ArgumentException("The array arguments must have the same length.");
            }

            return SafeNativeMethods.d_dot_product(_blasHandle, x.Length, x, y);
        }

        /// <summary>
        /// Does a point wise add of two arrays <c>z = x + y</c>. This can be used
        /// to add vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the addition.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public void AddArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] + y[i];
            }
        }

        /// <summary>
        /// Does a point wise subtraction of two arrays <c>z = x - y</c>. This can be used
        /// to subtract vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the subtraction.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public void SubtractArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] - y[i];
            }
        }

        /// <summary>
        /// Does a point wise multiplication of two arrays <c>z = x * y</c>. This can be used
        /// to multiple elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise multiplication.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public void PointWiseMultiplyArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] * y[i];
            }
        }

        /// <summary>
        /// Does a point wise division of two arrays <c>z = x / y</c>. This can be used
        /// to divide elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise division.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public void PointWiseDivideArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] / y[i];
            }
        }

        /// <summary>
        /// Does a point wise power of two arrays <c>z = x ^ y</c>. This can be used
        /// to raise elements of vectors or matrices to the powers of another vector or matrix.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise power.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public void PointWisePowerArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Pow(x[i], y[i]);
            }
        }

        /// <summary>
        /// Adds a scaled vector to another: <c>result = y + alpha*x</c>.
        /// </summary>
        /// <param name="y">The vector to update.</param>
        /// <param name="alpha">The value to scale <paramref name="x"/> by.</param>
        /// <param name="x">The vector to add to <paramref name="y"/>.</param>
        /// <param name="result">The result of the addition.</param>
        /// <remarks>This is similar to the AXPY BLAS routine.</remarks>
        [SecuritySafeCritical]
        public void AddVectorToScaledVector(double[] y, double alpha, double[] x, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y.Length != x.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            if (!ReferenceEquals(y, result))
            {
                Array.Copy(y, 0, result, 0, y.Length);
            }

            if (alpha == 0.0)
            {
                return;
            }

            SafeNativeMethods.d_axpy(_blasHandle, y.Length, alpha, x, result);
        }

        /// <summary>
        /// Scales an array. Can be used to scale a vector and a matrix.
        /// </summary>
        /// <param name="alpha">The scalar.</param>
        /// <param name="x">The values to scale.</param>
        /// <param name="result">This result of the scaling.</param>
        /// <remarks>This is similar to the SCAL BLAS routine.</remarks>
        [SecuritySafeCritical]
        public void ScaleArray(double alpha, double[] x, double[] result)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (!ReferenceEquals(x, result))
            {
                Array.Copy(x, 0, result, 0, x.Length);
            }

            if (alpha == 1.0)
            {
                return;
            }

            SafeNativeMethods.d_scale(_blasHandle, x.Length, alpha, result);
        }

        /// <summary>
        /// Conjugates an array. Can be used to conjugate a vector and a matrix.
        /// </summary>
        /// <param name="x">The values to conjugate.</param>
        /// <param name="result">This result of the conjugation.</param>
        public void ConjugateArray(double[] x, double[] result)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (!ReferenceEquals(x, result))
            {
                x.CopyTo(result, 0);
            }
        }

        /// <summary>
        /// Multiples two matrices. <c>result = x * y</c>
        /// </summary>
        /// <param name="x">The x matrix.</param>
        /// <param name="rowsX">The number of rows in the x matrix.</param>
        /// <param name="columnsX">The number of columns in the x matrix.</param>
        /// <param name="y">The y matrix.</param>
        /// <param name="rowsY">The number of rows in the y matrix.</param>
        /// <param name="columnsY">The number of columns in the y matrix.</param>
        /// <param name="result">Where to store the result of the multiplication.</param>
        /// <remarks>This is a simplified version of the BLAS GEMM routine with alpha
        /// set to 1.0 and beta set to 0.0, and x and y are not transposed.</remarks>
        public void MatrixMultiply(double[] x, int rowsX, int columnsX, double[] y, int rowsY, int columnsY, double[] result)
        {
            MatrixMultiplyWithUpdate(Transpose.DontTranspose, Transpose.DontTranspose, 1.0, x, rowsX, columnsX, y, rowsY, columnsY, 0.0, result);
        }

        /// <summary>
        /// Multiplies two matrices and updates another with the result. <c>c = alpha*op(a)*op(b) + beta*c</c>
        /// </summary>
        /// <param name="transposeA">How to transpose the <paramref name="a"/> matrix.</param>
        /// <param name="transposeB">How to transpose the <paramref name="b"/> matrix.</param>
        /// <param name="alpha">The value to scale <paramref name="a"/> matrix.</param>
        /// <param name="a">The a matrix.</param>
        /// <param name="rowsA">The number of rows in the <paramref name="a"/> matrix.</param>
        /// <param name="columnsA">The number of columns in the <paramref name="a"/> matrix.</param>
        /// <param name="b">The b matrix</param>
        /// <param name="rowsB">The number of rows in the <paramref name="b"/> matrix.</param>
        /// <param name="columnsB">The number of columns in the <paramref name="b"/> matrix.</param>
        /// <param name="beta">The value to scale the <paramref name="c"/> matrix.</param>
        /// <param name="c">The c matrix.</param>
        [SecuritySafeCritical]
        public void MatrixMultiplyWithUpdate(Transpose transposeA, Transpose transposeB, double alpha, double[] a, int rowsA, int columnsA, double[] b, int rowsB, int columnsB, double beta, double[] c)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            var m = transposeA == Transpose.DontTranspose ? rowsA : columnsA;
            var n = transposeB == Transpose.DontTranspose ? columnsB : rowsB;
            var k = transposeA == Transpose.DontTranspose ? columnsA : rowsA;
            var l = transposeB == Transpose.DontTranspose ? rowsB : columnsB;

            if (c.Length != m*n)
            {
                throw new ArgumentException("Matrix dimensions must agree.");
            }

            if (k != l)
            {
                throw new ArgumentException("Matrix dimensions must agree.");
            }

            SafeNativeMethods.d_matrix_multiply(_blasHandle, transposeA.ToCUDA(), transposeB.ToCUDA(), m, n, k, alpha, a, b, beta, c);
        }

        /// <summary>
        /// Computes the LUP factorization of A. P*A = L*U.
        /// </summary>
        /// <param name="data">An <paramref name="order"/> by <paramref name="order"/> matrix. The matrix is overwritten with the
        /// the LU factorization on exit. The lower triangular factor L is stored in under the diagonal of <paramref name="data"/> (the diagonal is always 1.0
        /// for the L factor). The upper triangular factor U is stored on and above the diagonal of <paramref name="data"/>.</param>
        /// <param name="order">The order of the square matrix <paramref name="data"/>.</param>
        /// <param name="ipiv">On exit, it contains the pivot indices. The size of the array must be <paramref name="order"/>.</param>
        /// <remarks>This is equivalent to the GETRF LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public void LUFactor(double[] data, int order, int[] ipiv)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException(nameof(ipiv));
            }

            if (data.Length != order*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(data));
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(ipiv));
            }

            Solver(SafeNativeMethods.d_lu_factor(_solverHandle, order, data, ipiv));
        }

        /// <summary>
        /// Computes the inverse of matrix using LU factorization.
        /// </summary>
        /// <param name="a">The N by N matrix to invert. Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <remarks>This is equivalent to the GETRF and GETRI LAPACK routines.</remarks>
        [SecuritySafeCritical]
        public void LUInverse(double[] a, int order)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(a));
            }

            Solver(SafeNativeMethods.d_lu_inverse(_solverHandle, _blasHandle, order, a));
        }

        /// <summary>
        /// Computes the inverse of a previously factored matrix.
        /// </summary>
        /// <param name="a">The LU factored N by N matrix.  Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <remarks>This is equivalent to the GETRI LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public void LUInverseFactored(double[] a, int order, int[] ipiv)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException(nameof(ipiv));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(a));
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(ipiv));
            }

            BLAS(SafeNativeMethods.d_lu_inverse_factored(_blasHandle, order, a, ipiv));
        }

        /// <summary>
        /// Solves A*X=B for X using LU factorization.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The square matrix A.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <remarks>This is equivalent to the GETRF and GETRS LAPACK routines.</remarks>
        [SecuritySafeCritical]
        public void LUSolve(int columnsOfB, double[] a, int order, double[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(a));
            }

            if (b.Length != columnsOfB*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException("Arguments must be different objects.");
            }

            Solver(SafeNativeMethods.d_lu_solve(_solverHandle, order, columnsOfB, a, b));
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The factored A matrix.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <remarks>This is equivalent to the GETRS LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public void LUSolveFactored(int columnsOfB, double[] a, int order, int[] ipiv, double[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException(nameof(ipiv));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(a));
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(ipiv));
            }

            if (b.Length != columnsOfB*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException("Arguments must be different objects.");
            }

            Solver(SafeNativeMethods.d_lu_solve_factored(_solverHandle, order, columnsOfB, a, ipiv, b));
        }

        /// <summary>
        /// Computes the Cholesky factorization of A.
        /// </summary>
        /// <param name="a">On entry, a square, positive definite matrix. On exit, the matrix is overwritten with the
        /// the Cholesky factorization.</param>
        /// <param name="order">The number of rows or columns in the matrix.</param>
        /// <remarks>This is equivalent to the POTRF LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public void CholeskyFactor(double[] a, int order)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (order < 1)
            {
                throw new ArgumentException("Value must be positive.", nameof(order));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(a));
            }

            Solver(SafeNativeMethods.d_cholesky_factor(_solverHandle, order, a));
        }

        /// <summary>
        /// Solves A*X=B for X using Cholesky factorization.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRF add POTRS LAPACK routines.
        /// </remarks>
        [SecuritySafeCritical]
        public void CholeskySolve(double[] a, int orderA, double[] b, int columnsB)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (b.Length != orderA*columnsB)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException("Arguments must be different objects.");
            }

            Solver(SafeNativeMethods.d_cholesky_solve(_solverHandle, orderA, columnsB, a, b));
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRS LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public void CholeskySolveFactored(double[] a, int orderA, double[] b, int columnsB)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (b.Length != orderA*columnsB)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException("Arguments must be different objects.");
            }

            Solver(SafeNativeMethods.d_cholesky_solve_factored(_solverHandle, orderA, columnsB, a, b));
        }

        /// <summary>
        /// Computes the QR factorization of A.
        /// </summary>
        /// <param name="r">On entry, it is the M by N A matrix to factor. On exit,
        /// it is overwritten with the R matrix of the QR factorization. </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="q">On exit, A M by M matrix that holds the Q matrix of the
        /// QR factorization.</param>
        /// <param name="tau">A min(m,n) vector. On exit, contains additional information
        /// to be used by the QR solve routine.</param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        public void QRFactor(double[] r, int rowsR, int columnsR, double[] q, double[] tau)
        {
            ManagedLinearAlgebraProvider.Instance.QRFactor(r, rowsR, columnsR, q, tau);
        }

        /// <summary>
        /// Computes the thin QR factorization of A where M &gt; N.
        /// </summary>
        /// <param name="q">On entry, it is the M by N A matrix to factor. On exit,
        /// it is overwritten with the Q matrix of the QR factorization.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="r">On exit, A N by N matrix that holds the R matrix of the
        /// QR factorization.</param>
        /// <param name="tau">A min(m,n) vector. On exit, contains additional information
        /// to be used by the QR solve routine.</param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        public void ThinQRFactor(double[] q, int rowsA, int columnsA, double[] r, double[] tau)
        {
            ManagedLinearAlgebraProvider.Instance.ThinQRFactor(q, rowsA, columnsA, r, tau);
        }

        /// <summary>
        /// Solves A*X=B for X using QR factorization of A.
        /// </summary>
        /// <param name="a">The A matrix.</param>
        /// <param name="rows">The number of rows in the A matrix.</param>
        /// <param name="columns">The number of columns in the A matrix.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        /// <param name="method">The type of QR factorization to perform. <seealso cref="QRMethod"/></param>
        /// <remarks>Rows must be greater or equal to columns.</remarks>
        public void QRSolve(double[] a, int rows, int columns, double[] b, int columnsB, double[] x, QRMethod method = QRMethod.Full)
        {
            ManagedLinearAlgebraProvider.Instance.QRSolve(a, rows, columns, b, columnsB, x, method);
        }

        /// <summary>
        /// Solves A*X=B for X using a previously QR factored matrix.
        /// </summary>
        /// <param name="q">The Q matrix obtained by calling <see cref="QRFactor(double[],int,int,double[],double[])"/>.</param>
        /// <param name="r">The R matrix obtained by calling <see cref="QRFactor(double[],int,int,double[],double[])"/>. </param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="tau">Contains additional information on Q. Only used for the native solver
        /// and can be <c>null</c> for the managed provider.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        /// <param name="method">The type of QR factorization to perform. <seealso cref="QRMethod"/></param>
        /// <remarks>Rows must be greater or equal to columns.</remarks>
        public void QRSolveFactored(double[] q, double[] r, int rowsA, int columnsA, double[] tau, double[] b, int columnsB, double[] x, QRMethod method = QRMethod.Full)
        {
            ManagedLinearAlgebraProvider.Instance.QRSolveFactored(q, r, rowsA, columnsA, tau, b, columnsB, x, method);
        }

        /// <summary>
        /// Solves A*X=B for X using the singular value decomposition of A.
        /// </summary>
        /// <param name="a">On entry, the M by N matrix to decompose.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public void SvdSolve(double[] a, int rowsA, int columnsA, double[] b, int columnsB, double[] x)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (b.Length != rowsA*columnsB)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(b));
            }

            if (x.Length != columnsA*columnsB)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(b));
            }

            var s = new double[Math.Min(rowsA, columnsA)];
            var u = new double[rowsA*rowsA];
            var vt = new double[columnsA*columnsA];

            var clone = new double[a.Length];
            a.Copy(clone);
            SingularValueDecomposition(true, clone, rowsA, columnsA, s, u, vt);
            SvdSolveFactored(rowsA, columnsA, s, u, vt, b, columnsB, x);
        }

        /// <summary>
        /// Solves A*X=B for X using a previously SVD decomposed matrix.
        /// </summary>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The s values returned by <see cref="SingularValueDecomposition(bool,double[],int,int,double[],double[],double[])"/>.</param>
        /// <param name="u">The left singular vectors returned by  <see cref="SingularValueDecomposition(bool,double[],int,int,double[],double[],double[])"/>.</param>
        /// <param name="vt">The right singular  vectors returned by  <see cref="SingularValueDecomposition(bool,double[],int,int,double[],double[],double[])"/>.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public void SvdSolveFactored(int rowsA, int columnsA, double[] s, double[] u, double[] vt, double[] b, int columnsB, double[] x)
        {
            ManagedLinearAlgebraProvider.Instance.SvdSolveFactored(rowsA, columnsA, s, u, vt, b, columnsB, x);
        }

        /// <summary>
        /// Computes the singular value decomposition of A.
        /// </summary>
        /// <param name="computeVectors">Compute the singular U and VT vectors or not.</param>
        /// <param name="a">On entry, the M by N matrix to decompose. On exit, A may be overwritten.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The singular values of A in ascending value.</param>
        /// <param name="u">If <paramref name="computeVectors"/> is <c>true</c>, on exit U contains the left
        /// singular vectors.</param>
        /// <param name="vt">If <paramref name="computeVectors"/> is <c>true</c>, on exit VT contains the transposed
        /// right singular vectors.</param>
        /// <remarks>This is equivalent to the GESVD LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public void SingularValueDecomposition(bool computeVectors, double[] a, int rowsA, int columnsA, double[] s, double[] u, double[] vt)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }

            if (vt == null)
            {
                throw new ArgumentNullException(nameof(vt));
            }

            if (u.Length != rowsA*rowsA)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(u));
            }

            if (vt.Length != columnsA*columnsA)
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(vt));
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException("The array arguments must have the same length.", nameof(s));
            }

            if (columnsA > rowsA || !computeVectors) // see remarks http://docs.nvidia.com/cuda/cusolver/index.html#cuds-lt-t-gt-gesvd
                ManagedLinearAlgebraProvider.Instance.SingularValueDecomposition(computeVectors, a, rowsA, columnsA, s, u, vt);
            else Solver (SafeNativeMethods.d_svd_factor(_solverHandle, computeVectors, rowsA, columnsA, a, s, u, vt));
        }

        /// <summary>
        /// Computes the eigenvalues and eigenvectors of a matrix.
        /// </summary>
        /// <param name="isSymmetric">Whether the matrix is symmetric or not.</param>
        /// <param name="order">The order of the matrix.</param>
        /// <param name="matrix">The matrix to decompose. The length of the array must be order * order.</param>
        /// <param name="matrixEv">On output, the matrix contains the eigen vectors. The length of the array must be order * order.</param>
        /// <param name="vectorEv">On output, the eigen values (λ) of matrix in ascending value. The length of the array must <paramref name="order"/>.</param>
        /// <param name="matrixD">On output, the block diagonal eigenvalue matrix. The length of the array must be order * order.</param>
        public void EigenDecomp(bool isSymmetric, int order, double[] matrix, double[] matrixEv, Complex[] vectorEv, double[] matrixD)
        {
            ManagedLinearAlgebraProvider.Instance.EigenDecomp(isSymmetric, order, matrix, matrixEv, vectorEv, matrixD);
        }
    }
}
