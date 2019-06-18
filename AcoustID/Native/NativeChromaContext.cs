
namespace AcoustID.Native
{
    using AcoustID.Chromaprint;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Wraps the native Chromaprint API.
    /// </summary>
    public class NativeChromaContext : IChromaContext, IDisposable
    {
        private const int OK = 1;

        private static string version;

        /// <summary>
        /// Returns the Chromaprint version.
        /// </summary>
        public static string GetVersion()
        {
            if (version == null)
            {
                try
                {
                    version = Marshal.PtrToStringAnsi(NativeMethods.chromaprint_get_version());
                }
                catch
                {
                    version = "N/A";
                }
            }

            return version;
        }

        private ChromaprintContext ctx;

        /// <summary>
        /// Gets the fingerprint algorithm this context is configured to use.
        /// </summary>
        public int Algorithm { get; }

        /// <summary>
        /// Gets the Chromaprint version.
        /// </summary>
        public string Version
        {
            get { return GetVersion(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeChromaContext" /> class.
        /// </summary>
        public NativeChromaContext()
            : this(ChromaprintAlgorithm.TEST2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeChromaContext" /> class.
        /// </summary>
        /// <param name="algorithm">The algorithm to use, see <see cref="ChromaprintAlgorithm" /> (default = TEST2)</param>
        public NativeChromaContext(ChromaprintAlgorithm algorithm)
        {
            this.Algorithm = (int)algorithm;

            ctx = NativeMethods.chromaprint_new(this.Algorithm);
        }
        
        ~NativeChromaContext()
        {
            Dispose(false);
        }

        #region Audio consumer interface

        /// <summary>
        /// Send audio data to the fingerprint calculator (alias to Feed() method).
        /// </summary>
        /// <param name="data">raw audio data, should point to an array of 16-bit 
        /// signed integers in native byte-order</param>
        /// <param name="size">size of the data buffer (in samples)</param>
        public void Consume(short[] data, int size)
        {
            Feed(data, size);
        }

        #endregion

        /// <summary>
        /// Restart the computation of a fingerprint with a new audio stream.
        /// </summary>
        /// <param name="sample_rate">Sample rate of the audio stream (in Hz).</param>
        /// <param name="num_channels">Numbers of channels in the audio stream (1 or 2).</param>
        /// <returns>False on error, true on success.</returns>
        public bool Start(int sample_rate, int num_channels)
        {
            return NativeMethods.chromaprint_start(ctx, sample_rate, num_channels) == OK;
        }

        /// <summary>
        /// Send audio data to the fingerprint calculator.
        /// </summary>
        /// <param name="data">Raw audio data, should point to an array of 16-bit 
        /// signed integers in native byte-order.</param>
        /// <param name="size">Size of the data buffer (in samples).</param>
        public void Feed(short[] data, int size)
        {
            var h = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                if (NativeMethods.chromaprint_feed(ctx, h.AddrOfPinnedObject(), size) != OK)
                {
                    // throw?
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h.Free();
            }
        }

        /// <summary>
        /// Process any remaining buffered audio data and calculate the fingerprint.
        /// </summary>
        public void Finish()
        {
            if (NativeMethods.chromaprint_finish(ctx) != OK)
            {
                // throw?
            }
        }

        /// <summary>
        /// Return the calculated fingerprint as a compressed string.
        /// </summary>
        /// <returns>The fingerprint as a compressed string.</returns>
        public string GetFingerprint()
        {
            if (NativeMethods.chromaprint_get_fingerprint(ctx, out IntPtr fp) == OK)
            {
                string result = Marshal.PtrToStringAnsi(fp);

                NativeMethods.chromaprint_dealloc(fp);

                return result;
            }

            // throw?
            return null;
        }

        /// <summary>
        /// Return the calculated fingerprint as an array of 32-bit integers.
        /// </summary>
        /// <returns>The raw fingerprint (array of 32-bit integers).</returns>
        public int[] GetRawFingerprint()
        {
            if (NativeMethods.chromaprint_get_raw_fingerprint(ctx, out IntPtr fingerprint, out int size) == OK)
            {
                var fp = new int[size];

                Marshal.Copy(fingerprint, fp, 0, size);

                NativeMethods.chromaprint_dealloc(fingerprint);

                return fp;
            }

            // throw?
            return null;
        }

        /// <summary>
        /// Return 32-bit hash of the calculated fingerprint.
        /// </summary>
        /// <returns>The hash.</returns>
        public int GetFingerprintHash()
        {
            if (NativeMethods.chromaprint_get_fingerprint_hash(ctx, out int hash) == OK)
            {
                return hash;
            }

            // throw?
            return 0;
        }

        /// <summary>
        /// Clear the current fingerprint, but allow more data to be processed.
        /// </summary>
        /// <returns>False on error, true on success.</returns>
        /// <remarks>
        /// This is useful if you are processing a long stream and want to many smaller fingerprints,
        /// instead of waiting for the entire stream to be processed.
        /// </remarks>
        public bool Clear()
        {
            return NativeMethods.chromaprint_clear_fingerprint(ctx) == OK;
        }

        #region Static methods

        /// <summary>
        /// Compress and optionally base64-encode a raw fingerprint.
        /// </summary>
        /// <param name="fp">Pointer to an array of 32-bit integers representing the raw fingerprint to be encoded.</param>
        /// <param name="algorithm">Chromaprint algorithm version which was used to generate the raw fingerprint.</param>
        /// <param name="base64">Whether to return binary data or base64-encoded ASCII data.</param>
        /// <returns>The encoded fingerprint.</returns>
        public static byte[] EncodeFingerprint(int[] fp, int algorithm, bool base64)
        {
            var h = GCHandle.Alloc(fp, GCHandleType.Pinned);
            
            try
            {
                var p = h.AddrOfPinnedObject();

                if (NativeMethods.chromaprint_encode_fingerprint(p, fp.Length, algorithm, out IntPtr encoded, out int size, base64 ? 1 : 0) == OK)
                {
                    var buffer = new byte[size];

                    Marshal.Copy(encoded, buffer, 0, size);

                    NativeMethods.chromaprint_dealloc(encoded);

                    return buffer;
                }

                // throw?
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h.Free();
            }
        }

        /// <summary>
        /// Uncompress and optionally base64-decode an encoded fingerprint.
        /// </summary>
        /// <param name="encoded">Pointer to an encoded fingerprint.</param>
        /// <param name="base64">Whether the encoded_fp parameter contains binary data or base64-encoded ASCII data.</param>
        /// <param name="algorithm">Chromaprint algorithm version which was used to generate the raw fingerprint.</param>
        /// <returns>The decoded raw fingerprint (array of 32-bit integers).</returns>
        public static int[] DecodeFingerprint(byte[] encoded, bool base64, out int algorithm)
        {
            var h = GCHandle.Alloc(encoded, GCHandleType.Pinned);

            try
            {
                var p = h.AddrOfPinnedObject();

                if (NativeMethods.chromaprint_decode_fingerprint(p, encoded.Length, out IntPtr fp, out int size, out algorithm, base64 ? 1 : 0) == OK)
                {
                    var buffer = new int[size];

                    Marshal.Copy(fp, buffer, 0, size);

                    NativeMethods.chromaprint_dealloc(fp);

                    return buffer;
                }

                // throw?
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h.Free();
            }
        }

        /// <summary>
        /// Return 32-bit hash of the calculated fingerprint.
        /// </summary>
        /// <param name="fingerprint">Array of 32-bit integers representing the raw fingerprint to be encoded.</param>
        /// <returns>The hash.</returns>
        public static int HashFingerprint(int[] fingerprint)
        {
            var h = GCHandle.Alloc(fingerprint, GCHandleType.Pinned);

            try
            {
                NativeMethods.chromaprint_hash_fingerprint(h.AddrOfPinnedObject(), fingerprint.Length, out int hash);

                return hash;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h.Free();
            }
        }

        #endregion

        #region IDisposable

        // See https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

        bool disposed = false;

        /// <summary>
        /// Free unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Free unmanaged resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            // Free unmanaged objects.

            if (ctx.Pointer != IntPtr.Zero)
            {
                NativeMethods.chromaprint_free(ctx);

                ctx.Pointer = IntPtr.Zero;
            }

            disposed = true;
        }

        #endregion
    }
}
