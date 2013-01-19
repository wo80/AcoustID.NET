using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class ChromaTest
    {
        [TestMethod]
        public void TestNormalA()
        {
            FeatureVectorBuffer buffer = new FeatureVectorBuffer();
            Chroma chroma = new Chroma(10, 510, 256, 1000, buffer);
            FFTFrame frame = new FFTFrame(128);
            frame.Data[113] = 1.0;
            chroma.Consume(frame);
            Assert.AreEqual(12, buffer.features.Length);
            double[] expected_features = {
		        1.0, 0.0, 0.0, 0.0, 0.0, 0.0,
		        0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
	        };
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(expected_features[i], buffer.features[i], 0.0001);
            }
        }

        [TestMethod]
        public void TestNormalGSharp()
        {
            FeatureVectorBuffer buffer = new FeatureVectorBuffer();
            Chroma chroma = new Chroma(10, 510, 256, 1000, buffer);
            FFTFrame frame = new FFTFrame(128);
            frame.Data[112] = 1.0;
            chroma.Consume(frame);
            Assert.AreEqual(12, buffer.features.Length);
            double[] expected_features = {
		        0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
		        0.0, 0.0, 0.0, 0.0, 0.0, 1.0,
	        };
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(expected_features[i], buffer.features[i], 0.0001);
            }
        }

        [TestMethod]
        public void TestNormalB()
        {
            FeatureVectorBuffer buffer = new FeatureVectorBuffer();
            Chroma chroma = new Chroma(10, 510, 256, 1000, buffer);
            FFTFrame frame = new FFTFrame(128);
            frame.Data[64] = 1.0; // 250 Hz
            chroma.Consume(frame);
            Assert.AreEqual(12, buffer.features.Length);
            double[] expected_features = {
		        0.0, 0.0, 1.0, 0.0, 0.0, 0.0,
		        0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
	        };
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(expected_features[i], buffer.features[i], 0.0001);
            }
        }

        [TestMethod]
        public void TestInterpolatedB()
        {
            FeatureVectorBuffer buffer = new FeatureVectorBuffer();
            Chroma chroma = new Chroma(10, 510, 256, 1000, buffer);
            chroma.Interpolate = true;
            FFTFrame frame = new FFTFrame(128);
            frame.Data[64] = 1.0;
            chroma.Consume(frame);
            Assert.AreEqual(12, buffer.features.Length);
            double[] expected_features = {
		        0.0, 0.286905, 0.713095, 0.0, 0.0, 0.0,
		        0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
	        };
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(expected_features[i], buffer.features[i], 0.0001);
            }
        }

        [TestMethod]
        public void TestInterpolatedA()
        {
            FeatureVectorBuffer buffer = new FeatureVectorBuffer();
            Chroma chroma = new Chroma(10, 510, 256, 1000, buffer);
            chroma.Interpolate = true;
            FFTFrame frame = new FFTFrame(128);
            frame.Data[113] = 1.0;
            chroma.Consume(frame);
            Assert.AreEqual(12, buffer.features.Length);
            double[] expected_features = {
		        0.555242, 0.0, 0.0, 0.0, 0.0, 0.0,
		        0.0, 0.0, 0.0, 0.0, 0.0, 0.444758,
	        };
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(expected_features[i], buffer.features[i], 0.0001);
            }
        }

        [TestMethod]
        public void TestInterpolatedGSharp()
        {
            FeatureVectorBuffer buffer = new FeatureVectorBuffer();
            Chroma chroma = new Chroma(10, 510, 256, 1000, buffer);
            chroma.Interpolate = true;
            FFTFrame frame = new FFTFrame(128);
            frame.Data[112] = 1.0;
            chroma.Consume(frame);
            Assert.AreEqual(12, buffer.features.Length);
            double[] expected_features = {
		        0.401354, 0.0, 0.0, 0.0, 0.0, 0.0,
		        0.0, 0.0, 0.0, 0.0, 0.0, 0.598646,
	        };
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(expected_features[i], buffer.features[i], 0.0001);
            }
        }
    }
}
