using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode.DataTypes
{
	public struct Point2DLarge
	{
		public BigInteger X { get; set; }
		public BigInteger Y { get; set; }

		public Point2DLarge(BigInteger[] xy)
		{
			_ = xy ?? throw new ArgumentNullException(nameof(xy));
			if (xy.Length != 2)
			{
				throw new ArgumentException("Points must be of 2 elements.", nameof(xy));
			}
			X = xy[0];
			Y = xy[1];
		}

		public Point2DLarge(BigInteger x, BigInteger y)
		{
			X = x;
			Y = y;
		}

		public Point2DLarge(string str)
		{
			var parts = str.Split(new char[] { ',' });
			X = int.Parse(parts[0]);
			Y = int.Parse(parts[1]);
		}

		public static Point2DLarge operator -(Point2DLarge p1)
		{
			return new Point2DLarge(-p1.X, -p1.Y);
		}

		public static Point2DLarge operator /(Point2DLarge p1, int denom)
		{
			return new Point2DLarge(p1.X / denom, p1.Y / denom);
		}

		public static Point2DLarge operator %(Point2DLarge p1, Point2DLarge p2)
		{
			return new Point2DLarge(p1.X % p2.X, p1.Y % p2.Y);
		}

		public static Point2DLarge operator %(Point2DLarge p1, int p2)
		{
			return new Point2DLarge(p1.X % p2, p1.Y % p2);
		}

		public static Point2DLarge operator +(Point2DLarge p1, Point2DLarge p2)
		{
			return new Point2DLarge(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point2DLarge operator -(Point2DLarge p1, Point2DLarge p2)
		{
			return new Point2DLarge(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point2DLarge operator *(Point2DLarge p1, BigInteger mul)
		{
			return new Point2DLarge(p1.X * mul, p1.Y * mul);
		}

		public static Point2DLarge operator *(BigInteger mul, Point2DLarge p1)
		{
			return new Point2DLarge(p1.X * mul, p1.Y * mul);
		}

		public static bool operator ==(Point2DLarge p1, Point2DLarge p2)
		{
			return (p1.X == p2.X) && (p1.Y == p2.Y);
		}

		public static bool operator ==(Point2DLarge p1, BigInteger p2)
		{
			return (p1.X == p2) && (p1.Y == p2);
		}

		public static bool operator !=(Point2DLarge p1, Point2DLarge p2)
		{
			return (p1.X != p2.X) || (p1.Y != p2.Y);
		}

		public static bool operator !=(Point2DLarge p1, BigInteger p2)
		{
			return (p1.X != p2) || (p1.Y != p2);
		}

		public static bool operator <(Point2DLarge p1, BigInteger p2)
		{
			return (p1.X < p2) && (p1.Y < p2);
		}

		public static bool operator >(Point2DLarge p1, BigInteger p2)
		{
			return (p1.X > p2) && (p1.Y > p2);
		}

		public static bool operator <=(Point2DLarge p1, BigInteger p2)
		{
			return (p1.X <= p2) && (p1.Y <= p2);
		}

		public static bool operator >=(Point2DLarge p1, BigInteger p2)
		{
			return (p1.X >= p2) && (p1.Y >= p2);
		}

		public override string ToString()
		{
			return $"{X}, {Y}";
		}

		public static readonly Point2DLarge Zero = new Point2DLarge(0, 0);
		public static readonly Point2DLarge One = new Point2DLarge(1, 1);
		public static readonly Point2DLarge Up = new Point2DLarge(0, -1);
		public static readonly Point2DLarge Down = new Point2DLarge(0, 1);
		public static readonly Point2DLarge Left = new Point2DLarge(-1, 0);
		public static readonly Point2DLarge Right = new Point2DLarge(1, 0);
		public static readonly Point2DLarge UpLeft = new Point2DLarge(-1, -1);
		public static readonly Point2DLarge UpRight = new Point2DLarge(1, -1);
		public static readonly Point2DLarge DownLeft = new Point2DLarge(-1, 1);
		public static readonly Point2DLarge DownRight = new Point2DLarge(1, 1);

		public override bool Equals(object? obj)
		{
			if (base.Equals(obj))
			{
				return this == ((Point2DLarge)obj);
			}

			return false;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public static readonly IReadOnlyList<Point2DLarge> OrthoDirections = new Point2DLarge[]
		{
			Up,
			Right,
			Down,
			Left,
		};

		public static readonly IReadOnlyList<Point2DLarge> Directions = new Point2DLarge[]
		{
			Up,
			UpRight,
			Right,
			DownRight,
			Down,
			DownLeft,
			Left,
			UpLeft,
		};
	}
}
