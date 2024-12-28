using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataTypes
{
	public struct Point2D
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Point2D(int[] xy)
		{
			_ = xy ?? throw new ArgumentNullException(nameof(xy));
			if (xy.Length != 2)
			{
				throw new ArgumentException("Points must be of 2 elements.", nameof(xy));
			}
			X = xy[0];
			Y = xy[1];
		}

		public Point2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point2D(string str)
		{
			var parts = str.Split(new char[] { ',' });
			X = int.Parse(parts[0]);
			Y = int.Parse(parts[1]);
		}

		public static Point2D operator -(Point2D p1)
		{
			return new Point2D(-p1.X, -p1.Y);
		}

		public static Point2D operator /(Point2D p1, int denom)
		{
			return new Point2D(p1.X / denom, p1.Y / denom);
		}

		public static Point2D operator %(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X % p2.X, p1.Y % p2.Y);
		}

		public static Point2D operator %(Point2D p1, int p2)
		{
			return new Point2D(p1.X % p2, p1.Y % p2);
		}

		public static Point2D operator +(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point2D operator -(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point2D operator *(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X * p2.X, p1.Y * p2.Y);
		}

		public static Point2D operator *(Point2D p1, int mul)
		{
			return new Point2D(p1.X * mul, p1.Y * mul);
		}

		public static Point2D operator *(int mul, Point2D p1)
		{
			return new Point2D(p1.X * mul, p1.Y * mul);
		}

		public static bool operator ==(Point2D p1, Point2D p2)
		{
			return (p1.X == p2.X) && (p1.Y == p2.Y);
		}

		public static bool operator ==(Point2D p1, int p2)
		{
			return (p1.X == p2) && (p1.Y == p2);
		}

		public static bool operator !=(Point2D p1, Point2D p2)
		{
			return (p1.X != p2.X) || (p1.Y != p2.Y);
		}

		public static bool operator !=(Point2D p1, int p2)
		{
			return (p1.X != p2) || (p1.Y != p2);
		}

		public static bool operator <(Point2D p1, int p2)
		{
			return (p1.X < p2) && (p1.Y < p2);
		}

		public static bool operator >(Point2D p1, int p2)
		{
			return (p1.X > p2) && (p1.Y > p2);
		}

		public static bool operator <=(Point2D p1, int p2)
		{
			return (p1.X <= p2) && (p1.Y <= p2);
		}

		public static bool operator >=(Point2D p1, int p2)
		{
			return (p1.X >= p2) && (p1.Y >= p2);
		}

		public Point2D Sign()
		{
			return new Point2D(
				Math.Sign(X),
				Math.Sign(Y)
			);
		}

		public int GetManhattenDistance(Point2D other)
		{
			return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y);
		}

		public bool IsOrthoAdjacent(Point2D point)
		{
			int distX = Math.Abs(point.X - X);
			int distY = Math.Abs(point.Y - Y);
			return ((distX <= 1) && (distY <= 1)) && ((distX ^ distY) == 1);
		}

		public bool IsAdjacent(Point2D point)
		{
			return (Math.Abs(point.X - X) <= 1) && (Math.Abs(point.Y - Y) <= 1);
		}

		public override string ToString()
		{
			return $"{X}, {Y}";
		}

		public static readonly Point2D Zero = new Point2D(0, 0);
		public static readonly Point2D One = new Point2D(1, 1);
		public static readonly Point2D Up = new Point2D(0, -1);
		public static readonly Point2D Down = new Point2D(0, 1);
		public static readonly Point2D Left = new Point2D(-1, 0);
		public static readonly Point2D Right = new Point2D(1, 0);
		public static readonly Point2D UpLeft = new Point2D(-1, -1);
		public static readonly Point2D UpRight = new Point2D(1, -1);
		public static readonly Point2D DownLeft = new Point2D(-1, 1);
		public static readonly Point2D DownRight = new Point2D(1, 1);

		public override bool Equals(object? obj)
		{
			if (base.Equals(obj))
			{
				return this == ((Point2D)obj);
			}

			return false;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public static readonly IReadOnlyList<Point2D> OrthoDirections = new Point2D[]
		{
			Up,
			Right,
			Down,
			Left,
		};

		public static readonly IReadOnlyList<Point2D> Directions = new Point2D[]
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
