using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataTypes
{
	public struct Point2D
	{
		public int X { get; set; }
		public int Y { get; set; }

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

		public static Point2D operator /(Point2D p1, int denom)
		{
			return new Point2D(p1.X / denom, p1.Y / denom);
		}

		public static Point2D operator +(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point2D operator -(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static bool operator ==(Point2D p1, Point2D p2)
		{
			return (p1.X == p2.X) && (p1.Y == p2.Y);
		}

		public static bool operator !=(Point2D p1, Point2D p2)
		{
			return (p1.X != p2.X) || (p1.Y != p2.Y);
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
			Point2D.Up,
			Point2D.Down,
			Point2D.Left,
			Point2D.Right,
		};
	}
}
