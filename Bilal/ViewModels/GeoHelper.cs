namespace Bilal
{
    using System;

    using Plugin.Geolocator.Abstractions;

    public class GeoHelper
    {
        private const double Radius = 6371;

        public static double ToRad(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double ToDeg(double angle)
        {
            return angle * 180 / Math.PI;
        }

        public static double BearingTo(double lat1, double lon1, double lat2, double lon2)
        {
            double dLon = ToRad(lon2 - lon1);

            lat1 = ToRad(lat1);
            lat2 = ToRad(lat2);

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

            double bearing = ToDeg(Math.Atan2(y, x));

            if (bearing < 0)
            {
                bearing += 360;
            }

            return bearing;
        }

        public static double BearingFrom(double lat1, double lon1, double lat2, double lon2)
        {
            double bearing = BearingTo(lat1, lon1, lat2, lon2);

            bearing += 180;
            if (bearing > 360)
            {
                bearing -= 360;
            }

            return bearing;
        }

        public static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);

            double a, c, distance;

            lat1 = ToRad(lat1);
            lat2 = ToRad(lat2);

            a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            distance = Radius * c;

            return distance;
        }
    }
}