using System;
using System.Collections.Generic;

namespace Biomet_Project
{
    public class Verifier
    {
        private List<double> m_Identity;
        public bool HasIdentity { get { return m_Identity != null; } }

        private const double MIN_DISTANCE = 20;

        public void SetIdentity(List<double> identity)
        {
            m_Identity = identity;
        }

        public bool Verify(List<double> newIdentity)
        {
            double dist = GetDistance(m_Identity, newIdentity);
            return dist < MIN_DISTANCE;
        }

        private double GetDistance(List<double> a, List<double> b)
        {
            double sqSum = 0.0;

            for (int i = 0; i < a.Count && i < b.Count; ++i)
            {
                sqSum += (a[i] - b[i]) * (a[i] - b[i]);
            }

            return Math.Sqrt(sqSum);
        }
    }
}