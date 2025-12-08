using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataTypes
{
	public struct Point3D
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public Point3D(int[] xyz)
		{
			_ = xyz ?? throw new ArgumentNullException(nameof(xyz));
			if (xyz.Length != 3)
			{
				throw new ArgumentException("Points must be of 3 elements.", nameof(xyz));
			}
			X = xyz[0];
			Y = xyz[1];
			Z = xyz[2];
		}

		public Point3D(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Point3D(string str)
		{
			var parts = str.Split([',']);
			X = int.Parse(parts[0]);
			Y = int.Parse(parts[1]);
			Z = int.Parse(parts[2]);
		}

		public static Point3D operator -(Point3D p1)
		{
			return new Point3D(-p1.X, -p1.Y, -p1.Z);
		}

		public static Point3D operator /(Point3D p1, int denom)
		{
			return new Point3D(p1.X / denom, p1.Y / denom, p1.Z / denom);
		}

		public static Point3D operator %(Point3D p1, Point3D p2)
		{
			return new Point3D(p1.X % p2.X, p1.Y % p2.Y, p1.Z % p2.Z);
		}

		public static Point3D operator %(Point3D p1, int p2)
		{
			return new Point3D(p1.X % p2, p1.Y % p2, p1.Z % p2);
		}

		public static Point3D operator +(Point3D p1, Point3D p2)
		{
			return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
		}

		public static Point3D operator -(Point3D p1, Point3D p2)
		{
			return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
		}

		public static Point3D operator *(Point3D p1, Point3D p2)
		{
			return new Point3D(p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
		}

		public static Point3D operator *(Point3D p1, int mul)
		{
			return new Point3D(p1.X * mul, p1.Y * mul, p1.Z * mul);
		}

		public static Point3D operator *(int mul, Point3D p1)
		{
			return new Point3D(p1.X * mul, p1.Y * mul, p1.Z * mul);
		}

		public static bool operator ==(Point3D p1, Point3D p2)
		{
			return (p1.X == p2.X) && (p1.Y == p2.Y) && (p1.Z == p2.Z);
		}

		public static bool operator ==(Point3D p1, int p2)
		{
			return (p1.X == p2) && (p1.Y == p2) && (p1.Z == p2);
		}

		public static bool operator !=(Point3D p1, Point3D p2)
		{
			return (p1.X != p2.X) || (p1.Y != p2.Y) || (p1.Z != p2.Z);
		}

		public static bool operator !=(Point3D p1, int p2)
		{
			return (p1.X != p2) || (p1.Y != p2) || (p1.Z != p2);
		}

		public static bool operator <(Point3D p1, int p2)
		{
			return (p1.X < p2) && (p1.Y < p2) && (p1.Z < p2);
		}

		public static bool operator >(Point3D p1, int p2)
		{
			return (p1.X > p2) && (p1.Y > p2) && (p1.Z > p2);
		}

		public static bool operator <=(Point3D p1, int p2)
		{
			return (p1.X <= p2) && (p1.Y <= p2) && (p1.Z <= p2);
		}

		public static bool operator >=(Point3D p1, int p2)
		{
			return (p1.X >= p2) && (p1.Y >= p2) && (p1.Z >= p2);
		}

		public Point3D Sign()
		{
			return new Point3D(
				Math.Sign(X),
				Math.Sign(Y),
				Math.Sign(Z)
			);
		}

		public float GetDistanceSquared(Point3D other)
		{
			return (float)(Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2) + Math.Pow(this.Z - other.Z, 2));
		}

		public float GetDistance(Point3D other)
		{
			return (float)Math.Sqrt(GetDistanceSquared(other));
		}

		public int GetManhattenDistance(Point3D other)
		{
			return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y) + Math.Abs(this.Z - other.Z);
		}

		public bool IsAdjacent(Point3D point)
		{
			return (Math.Abs(point.X - X) <= 1) && (Math.Abs(point.Y - Y) <= 1) && (Math.Abs(point.Z - Z) <= 1);
		}

		public override string ToString()
		{
			return $"{X}, {Y}, {Z}";
		}

		public override bool Equals(object? obj)
		{
			if (base.Equals(obj))
			{
				return this == ((Point3D)obj);
			}

			return false;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
