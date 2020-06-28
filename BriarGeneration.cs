using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Walls.Natural;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace SpiritMod
{
	public class BriarGeneration : ModWorld
	{
		private const float M_CAVE_RADIUS_TOP = 17f;
		private const float M_CAVE_RADIUS_BOTTOM = 1f;

		private int _x = 0, _y = 0;
		private Point _size;
		private Point _halfSize;
		private Point _topSize;
		private Vector2 _center;
		private Vector2 _caveTop;
		private Vector2 _caveBottom;
		private Vector2 _houseCenter;
		private float _houseGrassRadius;
		private PerlinNoise _noise;
		private List<float> _estimates;

		private struct BlockSpot
		{
			public int X;
			public int Y;
			public float Strength;
			public BlockSpot(int x, int y, float s)
			{
				X = x;
				Y = y;
				Strength = s;
			}
		}

		private class LinePoint
		{
			public Vector2 Point;
			public float Radius;
			public float Jaggedness;
			public LinePoint(float x, float y, float r, float j)
			{
				Point = new Vector2(x, y);
				Radius = r;
				Jaggedness = j;
			}
			public override string ToString()
			{
				return Point.X + ", " + Point.Y + " -- " + Radius + " -- " + Jaggedness;
			}
		}

		private class LineSection
		{
			public static readonly List<ushort> DO_NOT_BREAK = new List<ushort>()
			{
				TileID.BlueDungeonBrick,
				TileID.GreenDungeonBrick,
				TileID.PinkDungeonBrick,
				TileID.LihzahrdBrick
			};

			public LinePoint point1;
			public LinePoint point2;
			public float NoiseMultiplier;
			private List<float> _distanceEstimates;
			private Vector2 _center;
			private Vector2 _point1to2;
			private float _pointDistance;

			public LineSection(LinePoint c1, LinePoint c2, float noiseMult = 0.15f)
			{
				point1 = c1;
				point2 = c2;
				NoiseMultiplier = noiseMult;

				_point1to2 = (point2.Point - point1.Point);
				_pointDistance = _point1to2.Length();
				_center = point1.Point + _point1to2 * 0.5f;
				_distanceEstimates = new List<float>();
				_distanceEstimates.Add(0f);

				int amt = 80;

				float angle = -MathHelper.Pi;
				float per = MathHelper.TwoPi / amt;

				Vector2 previous = GetPointAroundLine(angle, point1, point2);

				for(int i = 0; i < amt; i++) {
					angle += per;
					Vector2 next = GetPointAroundLine(angle, point1, point2);
					if((ClosestPointOnLineToPoint(next, point1.Point, point2.Point) - next).Length() > Math.Max(point1.Radius, point2.Radius) + 10) {
						continue;
					}

					_distanceEstimates.Add(_distanceEstimates[_distanceEstimates.Count - 1] + Vector2.Distance(previous, next));
					previous = next;
				}
			}

			private float GetPerimeterFromZero(float angle)
			{
				int count = _distanceEstimates.Count;
				float anglePer = MathHelper.TwoPi / count;
				angle += MathHelper.Pi;
				int point = Math.Min(count - 1, (int)Math.Floor(angle / anglePer));
				float remainder = angle % anglePer;
				float remProgress = remainder / anglePer;
				float next = point + 1 >= count ? _distanceEstimates.Last() : _distanceEstimates[point + 1];
				return MathHelper.Lerp(_distanceEstimates[point], next, remProgress);
			}

			public void GetRadiusJaggednessAtClosestPointTo(Vector2 point, out float radius, out float jaggedness)
			{
				Vector2 closest = ClosestPointOnLineToPoint(point, point1.Point, point2.Point);
				float percent = (closest - point1.Point).Length() / _pointDistance;
				radius = MathHelper.Lerp(point1.Radius, point2.Radius, percent);
				jaggedness = MathHelper.Lerp(point1.Jaggedness, point2.Jaggedness, percent);
			}

			public bool ShouldClear(int x, int y, PerlinNoise noise, float noiseY)
			{
				Vector2 tile = new Vector2(x + 0.5f, y + 0.5f);
				Vector2 line = _center - tile;
				float angle = line.ToRotation();
				float perimeter = GetPerimeterFromZero(angle);
				Vector2 closest = ClosestPointOnLineToPoint(tile, point1.Point, point2.Point);
				GetRadiusJaggednessAtClosestPointTo(tile, out float radius, out float jaggedness);
				float distance = (tile - closest).Length();
				return distance < radius + noise.Noise(perimeter * NoiseMultiplier, noiseY) * jaggedness;
			}

			public void Carve(PerlinNoise noise)
			{
				float maxDist = Math.Max(point1.Radius + point1.Jaggedness, point2.Radius + point2.Jaggedness) + 4;
				int minX = (int)(Math.Min(point1.Point.X, point2.Point.X) - maxDist);
				int maxX = (int)(Math.Max(point1.Point.X, point2.Point.X) + maxDist);
				int minY = (int)(Math.Min(point1.Point.Y, point2.Point.Y) - maxDist);
				int maxY = (int)(Math.Max(point1.Point.Y, point2.Point.Y) + maxDist);
				for(int x = minX; x <= maxX; x++) {
					for(int y = minY; y <= maxY; y++) {
						if(ShouldClear(x, y, noise, maxDist)) {
							Tile tile = Framing.GetTileSafely(x, y);

							if(tile.active() && DO_NOT_BREAK.Contains(tile.type)) continue;

							bool wasActive = tile.active();
							tile.active(false);
							if(wasActive) {
								tile.wall = WallID.DirtUnsafe;
							}
						}
					}
				}
			}

			public void Place(PerlinNoise noise, ushort tileType, bool placeTile = true, bool wall = false, ushort wallType = 0)
			{
				float maxDist = Math.Max(point1.Radius + point1.Jaggedness, point2.Radius + point2.Jaggedness) + 4;
				int minX = (int)(Math.Min(point1.Point.X, point2.Point.X) - maxDist);
				int maxX = (int)(Math.Max(point1.Point.X, point2.Point.X) + maxDist);
				int minY = (int)(Math.Min(point1.Point.Y, point2.Point.Y) - maxDist);
				int maxY = (int)(Math.Max(point1.Point.Y, point2.Point.Y) + maxDist);
				for(int x = minX; x <= maxX; x++) {
					for(int y = minY; y <= maxY; y++) {
						if(ShouldClear(x, y, noise, maxDist)) {
							Tile tile = Framing.GetTileSafely(x, y);
							if(placeTile) {
								tile.active(true);
								tile.slope(0);
								tile.type = tileType;
							}
							if(wall) {
								tile.wall = wallType;
							}
						}
					}
				}
			}
		}

		private class RadiusLine
		{
			private LineSection[] _sections;
			public float Length { get; }

			public RadiusLine(float noiseMultiplier = 0.15f, params LinePoint[] points)
			{
				_sections = new LineSection[points.Length - 1];
				Length = 0f;

				for(int i = 1; i < points.Length; i++) {
					_sections[i - 1] = new LineSection(points[i - 1], points[i], noiseMultiplier);
					Length += (points[i - 1].Point - points[i].Point).Length();
				}
			}

			public void TotalPositionToSectionAndPosition(float position, out float localPosition, out LineSection section)
			{
				int sec = 0;
				float pos = position;
				for(; sec < _sections.Length; sec++) {
					float sectionLength = (_sections[sec].point2.Point - _sections[sec].point1.Point).Length();
					if(pos < sectionLength) {
						//we've found the right sec and pos values
						break;
					}
					pos -= sectionLength;
				}

				section = _sections[sec];
				localPosition = pos;
			}

			public void Carve(PerlinNoise noise)
			{
				foreach(LineSection section in _sections) {
					section.Carve(noise);
				}
			}

			public void Place(PerlinNoise noise, ushort tile, bool placeTile = true, bool wall = false, ushort wallType = 0)
			{
				foreach(LineSection section in _sections) {
					section.Place(noise, tile, placeTile, wall, wallType);
				}
			}
		}

		private static Vector2 ClosestPointOnLineToPoint(Vector2 point, Vector2 lineStart, Vector2 lineEnd, bool isLineInfinite = false)
		{
			Vector2 delta = Vector2.Normalize(lineEnd - lineStart);
			Vector2 pointToStart = point - lineStart;
			float dot = Vector2.Dot(pointToStart, delta);
			Vector2 returnPoint = lineStart + delta * dot;

			if(!isLineInfinite) {
				//clamp the return point so that it cannot be a point that doesn't exist on the line defined by start and end points.
				returnPoint.X = MathHelper.Clamp(returnPoint.X, Math.Min(lineStart.X, lineEnd.X), Math.Max(lineStart.X, lineEnd.X));
				returnPoint.Y = MathHelper.Clamp(returnPoint.Y, Math.Min(lineStart.Y, lineEnd.Y), Math.Max(lineStart.Y, lineEnd.Y));
			}

			return returnPoint;
		}

		private static Vector2 GetPointAroundLine(float angle, LinePoint point1, LinePoint point2)
		{
			Vector2 linep1p2 = point2.Point - point1.Point;
			Vector2 linep2p1 = point1.Point - point2.Point;

			Vector2 center = point1.Point + (point2.Point - point1.Point) * 0.5f;
			Vector2 lineEnd = center + angle.ToRotationVector2() * 2000f;

			//Check semi-circle arc collisions
			GetIntersectionPointsLineCircle(center, lineEnd, point1.Point, point1.Radius, out Vector2[] points1);
			if(points1.Length > 0) {
				Vector2 furthestPoint = Vector2.Zero;
				float furthestDist = 0;
				foreach(Vector2 point in points1) {
					float distance = (point - center).Length();
					if(distance > furthestDist) {
						furthestDist = distance;
						furthestPoint = point;
					}
				}
				return furthestPoint;
			}
			GetIntersectionPointsLineCircle(center, lineEnd, point2.Point, point2.Radius, out Vector2[] points2);
			if(points2.Length > 0) {
				Vector2 furthestPoint = Vector2.Zero;
				float furthestDist = 0;
				foreach(Vector2 point in points2) {
					float distance = (point - center).Length();
					if(distance > furthestDist) {
						furthestDist = distance;
						furthestPoint = point;
					}
				}
				return furthestPoint;
			}
			//check line collisions
			float diff = angle - (float)Math.Atan2(linep1p2.Y, linep1p2.X);
			if(diff < -MathHelper.Pi) diff += MathHelper.TwoPi;
			if(diff < 0) {
				Vector2 line1A = point1.Point + Vector2.Normalize(linep1p2.GetClockwise90()) * point1.Radius;
				Vector2 line1B = point2.Point + Vector2.Normalize(linep1p2.GetClockwise90()) * point2.Radius;
				return FindLineIntersection(center, lineEnd, line1A, line1B);
			} else {
				Vector2 line2A = point1.Point + Vector2.Normalize(linep1p2.GetAntiClockwise90()) * point1.Radius;
				Vector2 line2B = point2.Point + Vector2.Normalize(linep1p2.GetAntiClockwise90()) * point2.Radius;
				return FindLineIntersection(center, lineEnd, line2A, line2B);
			}
		}

		private static Vector2 FindLineIntersection(Vector2 s1, Vector2 e1, Vector2 s2, Vector2 e2)
		{
			float a1 = e1.Y - s1.Y;
			float b1 = s1.X - e1.X;
			float c1 = a1 * s1.X + b1 * s1.Y;

			float a2 = e2.Y - s2.Y;
			float b2 = s2.X - e2.X;
			float c2 = a2 * s2.X + b2 * s2.Y;

			float delta = a1 * b2 - a2 * b1;
			//If lines are parallel, the result will be (NaN, NaN).
			return delta == 0 ? new Vector2(float.NaN, float.NaN)
				: new Vector2((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
		}

		//based on https://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm
		private static void GetIntersectionPointsLineCircle(Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float circleRadius, out Vector2[] points)
		{
			Vector2 line = lineEnd - lineStart;
			float LAB = line.Length();
			Vector2 D = new Vector2(line.X / LAB, line.Y / LAB);
			Vector2 E = ClosestPointOnLineToPoint(circleCenter, lineStart, lineEnd);
			float t = (E - lineStart).Length();
			Vector2 EC = E - circleCenter;
			float LEC = EC.Length();

			//System.Windows.Forms.MessageBox.Show($"E: {E} D: {D} circleCenter: {circleCenter} lineStart: {lineStart}");

			if(LEC < circleRadius) {
				float dt = (float)Math.Sqrt(circleRadius * circleRadius - LEC * LEC);

				points = new Vector2[2];
				points[0] = new Vector2((t - dt) * D.X + lineStart.X, (t - dt) * D.Y + lineStart.Y);
				points[1] = new Vector2((t + dt) * D.X + lineStart.X, (t + dt) * D.Y + lineStart.Y);
				return;
			} else if(LEC == circleRadius) {
				points = new Vector2[1];
				points[0] = E;
				return;
			}
			points = new Vector2[0];
		}

		private float EllipseFormula(int x, int y, Vector2 center, Point size)
		{
			return EllipseFormula(new Vector2(x * 16f + 8f, y * 16f + 8f), center, size.ToVector2() * 16f);
		}

		private float EllipseFormula(Vector2 point, Vector2 ellipseCenter, Vector2 ellipseSize)
		{
			return (float)Math.Pow(point.X - ellipseCenter.X, 2) / (float)Math.Pow((ellipseSize.X * 0.5f), 2) +
				(float)Math.Pow(point.Y - ellipseCenter.Y, 2) / (float)Math.Pow((ellipseSize.Y * 0.5f), 2);
		}

		private bool WithinEllipseNoise(Vector2 point, Vector2 ellipseCenter, Vector2 ellipseSize, List<float> arcEstimates, PerlinNoise noise)
		{
			float arcAmount = (float)GetArcEllipse((float)Math.Atan2(point.Y - ellipseCenter.Y, point.X - ellipseCenter.X), arcEstimates);
			float rightSide = 1f - (1f + noise.Noise(arcAmount * 0.009f, 0f) * 0.5f) * (0.1f + noise.Noise(-arcAmount * 0.005f, 0.5f) * 0.08f);

			return EllipseFormula(point, ellipseCenter, ellipseSize) <= rightSide;
		}

		private bool WithinEllipseNoise(int x, int y, Vector2 center, Point size, List<float> arcEstimates, PerlinNoise noise)
		{
			return WithinEllipseNoise(new Vector2(x * 16f + 8f, y * 16f + 8f), center, size.ToVector2() * 16f, arcEstimates, noise);
		}

		private bool WithinEllipse(Vector2 point, Vector2 ellipseCenter, Vector2 ellipseSize)
		{
			return
				Math.Pow(point.X - ellipseCenter.X, 2) / Math.Pow((ellipseSize.X * 0.5f), 2) +
				Math.Pow(point.Y - ellipseCenter.Y, 2) / Math.Pow((ellipseSize.Y * 0.5f), 2) <= 1f;
		}

		private bool WithinEllipse(int x, int y, Vector2 center, Point size)
		{
			return WithinEllipse(new Vector2(x * 16f + 8f, y * 16f + 8f), center, size.ToVector2() * 16f);
		}

		private Vector2 PointOnEllipse(float angle, Vector2 ellipseCenter, Vector2 ellipseSize)
		{
			float a = ellipseSize.X * 0.5f;
			float b = ellipseSize.Y * 0.5f;
			float tanTheta = (float)Math.Tan(angle);
			float x = (a * b) / (float)Math.Sqrt(b * b + a * a * tanTheta * tanTheta);
			float y = x * tanTheta;

			if(angle > -MathHelper.PiOver2 && angle < MathHelper.PiOver2)
				return new Vector2(ellipseCenter.X + x, ellipseCenter.Y + y);
			return new Vector2(ellipseCenter.X - x, ellipseCenter.Y - y);
		}

		private float GetArcEllipse(float angle, List<float> estimates)
		{
			int count = estimates.Count;
			float anglePer = MathHelper.TwoPi / count;
			angle += MathHelper.Pi;
			int point = (int)Math.Floor(angle / anglePer);
			float remainder = angle % anglePer;
			float remProgress = remainder / anglePer;
			float next = point + 1 >= count ? estimates[0] : estimates[point + 1];
			return MathHelper.Lerp(estimates[point], next, remProgress);
		}

		private bool ActiveSolid(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);
			return tile.active() && Main.tileSolid[tile.type];
		}

		private int FindSurfaceAt(int x, int checkAmt = 15, bool ignoreTrees = true)
		{
			int surface = (int)Main.worldSurface;
			int airCount = 0;
			while(true) {
				Tile testTile = Framing.GetTileSafely(x, surface);
				if((!testTile.active() && testTile.wall == 0) || (ignoreTrees && (testTile.type == TileID.LivingWood || testTile.type == TileID.LeafBlock || testTile.type == TileID.Trees))) //TODO: ENSURE THESE ARE UPDATED FOR NEW BRIAR TREE?
				{
					airCount++;
					if(airCount >= checkAmt) {
						break;
					}
				} else {
					airCount = 0;
				}
				surface--;
				//if (surface <= (int)WorldGen.worldSurfaceLow) break;
			}
			return surface + checkAmt;
		}

		private bool IsOneOf(Tile tile, List<ushort> types, List<ushort> walls = null)
		{
			if(walls != null) {
				for(int i = 0; i < walls.Count; i++) {
					if(walls[i] == tile.wall) return true;
				}
			}
			if(tile.active()) {
				for(int i = 0; i < types.Count; i++) {
					if(types[i] == tile.type) return true;
				}
			}
			return false;
		}

		private void RecurseRoots(int depth, int maxDepth, RadiusLine cave, PerlinNoise noise, float minDistance, float maxDistance, float minLength, float maxLength, float minRadius, float maxRadius)
		{
			if(depth >= maxDepth) return;

			float length = cave.Length;

			IterateRoot(depth, maxDepth, cave, noise, true, length, minDistance, maxDistance, minLength, maxLength, minRadius, maxRadius);
			IterateRoot(depth, maxDepth, cave, noise, false, length, minDistance, maxDistance, minLength, maxLength, minRadius, maxRadius);
		}

		private void IterateRoot(int depth, int maxDepth, RadiusLine cave, PerlinNoise noise, bool clockwise, float length, float minDistance, float maxDistance, float minLength, float maxLength, float minRadius, float maxRadius)
		{
			float start = length * 0.1f;
			float max = length * 0.9f;
			for(float position = start; position < max; position += WorldGen.genRand.NextFloat(minDistance, maxDistance)) {
				cave.TotalPositionToSectionAndPosition(position, out float localPos, out LineSection section);
				Vector2 direction = Vector2.Normalize(section.point2.Point - section.point1.Point);

				LineSection root = TryPlaceRoot(section, noise, clockwise, direction, localPos, minLength, maxLength, minRadius, maxRadius);

				//Carve the new root
				root.Carve(noise);

				int newDepth = depth + 1;
				RecurseRoots(newDepth, maxDepth, root, noise, minDistance * 0.8f, maxDistance * 0.8f, minDistance / newDepth, minDistance * 1.2f / newDepth, root.point1.Radius * 0.2f, root.point1.Radius * 0.7f);
			}
		}

		private void RecurseRoots(int depth, int maxDepth, LineSection section, PerlinNoise noise, float minDistance, float maxDistance, float minLength, float maxLength, float minRadius, float maxRadius)
		{
			if(depth >= maxDepth) return;

			Vector2 between = section.point2.Point - section.point1.Point;
			float length = between.Length();
			between.Normalize();

			IterateRoot(depth, maxDepth, section, noise, true, length, between, minDistance, maxDistance, minLength, maxLength, minRadius, maxRadius);
			IterateRoot(depth, maxDepth, section, noise, false, length, between, minDistance, maxDistance, minLength, maxLength, minRadius, maxRadius);
		}

		private void IterateRoot(int depth, int maxDepth, LineSection section, PerlinNoise noise, bool clockwise, float length, Vector2 direction, float minDistance, float maxDistance, float minLength, float maxLength, float minRadius, float maxRadius)
		{
			float start = length * 0.1f;
			float max = length * 0.9f;
			for(float position = start; position < max; position += WorldGen.genRand.NextFloat(minDistance, maxDistance)) {
				LineSection root = TryPlaceRoot(section, noise, clockwise, direction, position, minLength, maxLength, minRadius, maxRadius);

				//Carve the new root
				root.Carve(noise);

				int newDepth = depth + 1;
				RecurseRoots(newDepth, maxDepth, root, noise, minDistance * 0.8f, maxDistance * 0.8f, minDistance / newDepth, minDistance * 1.2f / newDepth, root.point1.Radius * 0.2f, root.point1.Radius * 0.7f);
			}
		}

		private LineSection TryPlaceRoot(LineSection section, PerlinNoise noise, bool clockwise, Vector2 direction, float distance, float minLength, float maxLength, float minRadius, float maxRadius)
		{
			Vector2 rootStart = section.point1.Point + direction * distance;

			float angle = 0f;
			if(clockwise) {
				angle = direction.GetClockwise90().ToRotation() + WorldGen.genRand.NextFloat(MathHelper.PiOver4 * 0.2f, MathHelper.PiOver4 * 0.5f);
			} else {
				angle = direction.GetAntiClockwise90().ToRotation() - WorldGen.genRand.NextFloat(MathHelper.PiOver4 * 0.2f, MathHelper.PiOver4 * 0.5f);
			}

			float rootLength = WorldGen.genRand.NextFloat(minLength, maxLength);

			Vector2 rootEnd = rootStart + angle.ToRotationVector2() * rootLength;

			section.GetRadiusJaggednessAtClosestPointTo(rootStart, out float radius, out float jaggedness);
			float startRadius = MathHelper.Clamp(radius * WorldGen.genRand.NextFloat(0.6f, 0.9f), minRadius, maxRadius);

			LineSection root = new LineSection(
				new LinePoint(rootStart.X, rootStart.Y, startRadius, jaggedness * 0.7f),
				new LinePoint(rootEnd.X, rootEnd.Y, 0.2f, 0f), section.NoiseMultiplier * 0.7f);

			return root;
		}

		private void PlaceBriar()
		{
			//----------------------------------------------------------
			// DATA SETUP (IGNORE)
			//----------------------------------------------------------

			// PARAMETERS
			float tunnelCenterDistance = 0.25f; //25% from center
			float tunnelFlatCheckDistance = 0.2f;
			int pointCount = 40;
			int topThickness = 50;
			int stoneDensityDepth = 80;
			int moundHalfWidth = 35;
			int moundHeight = 32;
			List<ushort> ignoreTiles = new List<ushort>()
			{
				TileID.BlueDungeonBrick,
				TileID.GreenDungeonBrick,
				TileID.PinkDungeonBrick,
				TileID.SandstoneBrick,
				TileID.Hive,
				TileID.LihzahrdBrick,
				TileID.HardenedSand,
				TileID.Sandstone,
				TileID.ClayBlock,
				TileID.Copper,
				TileID.Tin,
				TileID.Iron,
				TileID.Lead,
				TileID.Silver,
				TileID.Tungsten,
				TileID.Gold,
				TileID.Platinum,
				TileID.Amethyst,
				TileID.Topaz,
				TileID.Ruby,
				TileID.Sapphire,
				TileID.Emerald,
				TileID.Diamond
			};
			List<ushort> ignoreWalls = new List<ushort>()
			{
				WallID.SandstoneBrick,
				WallID.BlueDungeonSlabUnsafe,
				WallID.BlueDungeonTileUnsafe,
				WallID.BlueDungeonUnsafe,
				WallID.GreenDungeonSlabUnsafe,
				WallID.GreenDungeonTileUnsafe,
				WallID.GreenDungeonUnsafe,
				WallID.PinkDungeonSlabUnsafe,
				WallID.PinkDungeonTileUnsafe,
				WallID.PinkDungeonUnsafe,
				WallID.LihzahrdBrickUnsafe,
				WallID.HiveUnsafe,
				WallID.HardenedSand,
				WallID.Sandstone
			};
			int mainCaveFromTop = 70;
			int mainCaveFromBottom = 50;
			//-

			_topSize = new Point(_size.X, topThickness * 2);
			_center = new Vector2(_x * 16f + 8f, _y * 16f + 8f);
			_noise = new PerlinNoise(WorldGen._genRandSeed);
			_estimates = new List<float>();
			int halfCount = pointCount / 2;
			float amt = MathHelper.TwoPi / pointCount;
			float a = -MathHelper.Pi;
			Vector2 previous = PointOnEllipse(a, _center, _topSize.ToVector2() * 16f);
			_estimates.Add(0);
			for(int i = 0; i < pointCount; i++) {
				a += amt;
				Vector2 next = PointOnEllipse(a, _center, i <= halfCount ? _topSize.ToVector2() * 16f : _size.ToVector2() * 16f);
				_estimates.Add(_estimates[i] + Vector2.Distance(previous, next));
				previous = next;
			}
			float denseY = _center.Y + stoneDensityDepth * 16f;
			float bottomY = _center.Y + _size.Y * 16f;
			float stoneCompY = bottomY - denseY;
			int[] surfaceY = new int[_halfSize.X * 2 + 1];
			int curX = 0;
			int flatDistX = (int)(_size.X * tunnelCenterDistance);
			int flatCheckX = (int)(_size.X * tunnelFlatCheckDistance);
			int flattestX = -1;
			int flattestXFlatness = 100000;
			List<BlockSpot> stoneSpots = new List<BlockSpot>();
			//----------------------------------------------------------

			//Shape pass
			for(int tileX = _x - _halfSize.X; tileX <= _x + _halfSize.X; tileX++) {
				int surface = FindSurfaceAt(tileX, 20);
				int prevSurface = surface;
				if(curX > 0) {
					prevSurface = surfaceY[curX - 1];
					if(Math.Abs(prevSurface - surface) > 3) {
						surface = (int)MathHelper.Lerp(surface, prevSurface, WorldGen.genRand.NextFloat(0.35f, 0.65f));
					}
				}

				//get the first tile on the surface
				surfaceY[curX++] = surface;

				//place tiles vertically
				for(int tileY = surface; tileY <= _y + _halfSize.Y; tileY++) {
					bool valid;

					if(tileY <= _y) valid = WithinEllipseNoise(tileX, tileY, _center, _topSize, _estimates, _noise);
					else valid = WithinEllipseNoise(tileX, tileY, _center, _size, _estimates, _noise);

					if(valid) {
						Tile tile = Framing.GetTileSafely(tileX, tileY);

						if(IsOneOf(tile, ignoreTiles, ignoreWalls))
							continue;

						tile.type = TileID.Dirt;
						tile.active(true);
						tile.slope(0);
						if(tileY != surface) {
							tile.wall = WallID.DirtUnsafe;
						}

						float stoneValue = EllipseFormula(tileX, tileY, _center, _size) + ((tileY * 16f - denseY) / stoneCompY);
						if(tileY + 20 >= Main.worldSurface && stoneValue > 0.5f && WorldGen.genRand.Next(6) == 0) {
							stoneSpots.Add(new BlockSpot(tileX, tileY, stoneValue));
						}
					}
				}
			}

			//Add stone
			foreach(BlockSpot spot in stoneSpots) {
				WorldGen.TileRunner(spot.X, spot.Y, WorldGen.genRand.NextFloat(spot.Strength * 4f), (int)WorldGen.genRand.NextFloat(spot.Strength * 13f), TileID.Stone);
			}

			//Find the flattest place
			int subX = _x - _halfSize.X;
			for(int tileX = _x - flatDistX; tileX <= _x + flatDistX; tileX++) {
				int surf = FindSurfaceAt(tileX, 15, false);
				int total = 0;
				for(int xTest = tileX - flatCheckX; xTest <= tileX + flatCheckX; xTest++) {
					total += Math.Abs(surf - surfaceY[xTest - subX]);
				}
				if(total < flattestXFlatness) {
					flattestXFlatness = total;
					flattestX = tileX;
				}
			}

			//Add mound
			int moundTop = surfaceY[flattestX - subX] - moundHeight;
			float sinAmt = 0;
			float amtPer = MathHelper.Pi / (moundHalfWidth * 2);
			for(int tileX = flattestX - moundHalfWidth; tileX <= flattestX + moundHalfWidth; tileX++) {
				float sin = (float)Math.Sin(sinAmt);
				int bottom = surfaceY[tileX - subX] + 1;
				//int bottom = FindSurfaceAt(tileX - subX) + 1;
				int top = (int)(MathHelper.Lerp(bottom, moundTop, sin) + _noise.Noise(sinAmt * 2.7f, 0.8f) * 4f);
				for(int tileY = top; tileY <= bottom; tileY++) {
					Main.tile[tileX, tileY].active(true);
					Main.tile[tileX, tileY].type = TileID.Dirt;
					Main.tile[tileX, tileY].slope(0);
					if(tileY != top) {
						Main.tile[tileX, tileY].wall = WallID.DirtUnsafe;
					}
				}
				sinAmt += amtPer;
			}

			//add random spikes
			int spikes = _size.X / 50;
			for(int i = 0; i < spikes; i++) {
				int spikeX = WorldGen.genRand.Next(_x - _halfSize.X, _x + _halfSize.X);
				if(spikeX > flattestX - moundHalfWidth && spikeX < flattestX + moundHalfWidth) {
					i--;
					continue;
				}
				Vector2 top = new Vector2(spikeX, FindSurfaceAt(spikeX, 5));
				LinePoint spikeTopPoint = new LinePoint(top.X, top.Y, WorldGen.genRand.NextFloat(1f, 4f), 3f);
				Vector2 bottom = top + new Vector2(0f, WorldGen.genRand.NextFloat(15f, 40f)).RotatedByRandom(0.5);
				LinePoint bottomPoint = new LinePoint(bottom.X, bottom.Y, 0.1f, 0.1f);
				LineSection section = new LineSection(spikeTopPoint, bottomPoint);
				section.Carve(_noise);
				section.Place(_noise, 0, false, true, 0);
			}

			//Create main cave base points
			Vector2 mainTop = new Vector2(_x, _y + mainCaveFromTop);
			_caveTop = mainTop;
			Vector2 mainBottom = new Vector2(_x, _y + _halfSize.Y - mainCaveFromBottom);
			_caveBottom = mainBottom; //added these variables afterwards so lazy but eh
			Vector2 between = mainBottom - mainTop;
			LinePoint mainTopPoint = new LinePoint(mainTop.X, mainTop.Y, M_CAVE_RADIUS_TOP, 5f);
			LinePoint mainTopPointMinor = new LinePoint(mainTop.X, mainTop.Y, M_CAVE_RADIUS_TOP * 0.5f, 5f);

			//Create zig zag
			LineSection mainSectionTemp = new LineSection(mainTopPoint, new LinePoint(mainBottom.X, mainBottom.Y, M_CAVE_RADIUS_BOTTOM, 2f));
			int zigZags = WorldGen.genRand.Next(4, 8);
			Vector2 mainDir = Vector2.Normalize(between);
			float mainSectionLength = between.Length();
			float perZig = mainSectionLength / zigZags;
			float pos = 0f;
			List<LinePoint> zigPoints = new List<LinePoint>() { mainTopPoint };
			float zigAmount = _halfSize.X * WorldGen.genRand.NextFloat(0.02f, 0.05f);
			bool zigLeft = WorldGen.genRand.NextBool();
			for(int i = 0; i < zigZags; i++) {
				pos += perZig;
				Vector2 point = mainTop + mainDir * pos;
				mainSectionTemp.GetRadiusJaggednessAtClosestPointTo(point, out float radius, out float jaggedness);

				zigPoints.Add(new LinePoint(point.X + (zigLeft ? -zigAmount : zigAmount), point.Y, radius, jaggedness));

				zigLeft = !zigLeft;
			}
			zigPoints.Add(mainSectionTemp.point2);

			//Create and carve main cave
			RadiusLine mainCave = new RadiusLine(0.15f, zigPoints.ToArray());
			mainCave.Carve(_noise);

			//Mound point
			LinePoint moundMiddle = new LinePoint(flattestX, FindSurfaceAt(flattestX) + moundHeight * 0.6f, Math.Min(moundHalfWidth, moundHeight * 0.5f) * WorldGen.genRand.NextFloat(0.24f, 0.46f), 4f);

			//Mound exit point
			float exitOffX = WorldGen.genRand.NextFloat(moundHalfWidth * 0.8f, moundHalfWidth * 1.4f);
			if(WorldGen.genRand.NextBool()) exitOffX *= -1f;
			float exitOffY = WorldGen.genRand.NextFloat(-moundHeight * 0.4f, 0f);
			LinePoint moundExit = new LinePoint(moundMiddle.Point.X + exitOffX, moundMiddle.Point.Y + exitOffY, moundMiddle.Radius * 1.3f, moundMiddle.Jaggedness);

			//Turn point
			LinePoint turnPoint = new LinePoint(moundMiddle.Point.X, mainTopPoint.Point.Y - WorldGen.genRand.NextFloat(mainTopPoint.Point.Y - moundMiddle.Point.Y) * 0.25f, moundMiddle.Radius * WorldGen.genRand.NextFloat(0.7f, 0.9f), 5f);

			LineSection c1 = new LineSection(moundExit, moundMiddle, 0.08f);
			LineSection c2 = new LineSection(moundMiddle, turnPoint, 0.06f);
			LineSection c3 = new LineSection(turnPoint, mainTopPointMinor, 0.1f);

			c1.Carve(_noise);
			c2.Carve(_noise);
			c3.Carve(_noise);

			//place vinewarth hizouse
			bool leftSide = _x < WorldGen.dungeonX ? true : false;
			float hizouseY = mainBottom.Y - WorldGen.genRand.NextFloat(2f, 20f);
			for(int testX = (int)(mainBottom.X - 100); testX < (int)(mainBottom.X + 100); testX++) {
				Tile tile = Framing.GetTileSafely(testX, (int)hizouseY);
				if(tile.active() && (tile.type == TileID.BlueDungeonBrick || tile.type == TileID.GreenDungeonBrick || tile.type == TileID.PinkDungeonBrick || tile.type == TileID.LihzahrdBrick)) {
					hizouseY -= 30;
				}
			}
			int distanceFromMain = _halfSize.X / 2;
			int width = WorldGen.genRand.Next(9);
			float leftYOffset = WorldGen.genRand.NextFloat(-6, 6);
			float startRadius = WorldGen.genRand.NextFloat(44f, 50f); //<------------ THIS HERE
			float endRadius = startRadius * WorldGen.genRand.NextFloat(0.9f, 1.1f);
			float hizouseX = mainBottom.X + (leftSide ? -distanceFromMain : distanceFromMain);
			if(leftSide) hizouseX -= width;
			LineSection houseSection = new LineSection(new LinePoint(hizouseX, hizouseY + leftYOffset, startRadius, 3f), new LinePoint(hizouseX + width, hizouseY, endRadius, 3f));
			houseSection.Place(_noise, TileID.Dirt);
			houseSection.point1.Radius *= 0.7f;
			houseSection.point2.Radius *= 0.7f;
			houseSection.Carve(_noise);

			_houseCenter = new Vector2(hizouseX + width * 0.5f, hizouseY + leftYOffset * 0.5f);
			_houseGrassRadius = (Math.Max(startRadius, endRadius) + width * 0.5f) * 0.95f;

			//place extra blobs
			float angle = -MathHelper.Pi;
			for(; angle < MathHelper.Pi; angle += WorldGen.genRand.NextFloat(0.2f, 0.7f)) {
				if(WorldGen.genRand.NextBool(3)) continue;
				Vector2 rot = angle.ToRotationVector2();
				Vector2 center = _houseCenter + rot * _houseGrassRadius;
				LinePoint point = new LinePoint(center.X, center.Y, WorldGen.genRand.NextFloat(4f, 7f), 3f);
				LineSection sec = new LineSection(point, point);
				sec.Carve(_noise);
			}

			//place pathway to hizouse
			Vector2 start = mainBottom;
			Vector2 end = new Vector2(hizouseX + width * 0.5f, hizouseY);
			Vector2 housePath = end - start;
			Vector2 middle = start + housePath * 0.5f;
			float distToBottom = Math.Max(start.Y, end.Y) - middle.Y;
			middle.Y += distToBottom * WorldGen.genRand.NextFloat(1.6f, 2.5f);
			BezierCurve housePathCurve = new BezierCurve(start, middle, end);
			int points = 12;
			List<Vector2> pathCurvePoints = housePathCurve.GetPoints(points);
			LinePoint[] pathPoints = new LinePoint[pathCurvePoints.Count];
			float pathStartRadius = 2f;
			float pathEndRadius = startRadius * 0.17f;
			for(int i = 0; i < pathCurvePoints.Count; i++) {
				float progress = i / (float)pathCurvePoints.Count;
				Vector2 current = pathCurvePoints[i];
				pathPoints[i] = new LinePoint(current.X, current.Y, MathHelper.Lerp(pathStartRadius, pathEndRadius, progress), 2f);
			}
			RadiusLine path = new RadiusLine(0.15f, pathPoints);
			path.Carve(_noise);

			//Do cave placement
			RecurseRoots(0, 3, mainCave, _noise, mainSectionLength * 0.15f, mainSectionLength * 0.4f, _halfSize.X * 0.7f, _halfSize.X * 0.9f, 3f, M_CAVE_RADIUS_TOP * 0.6f);

			//pillar
			float pillarHeight = MathHelper.Lerp(startRadius, endRadius, 0.5f) * 0.6f;
			float bottomOfHole = hizouseY + leftYOffset * 0.5f + (Math.Min(startRadius, endRadius)) * 0.8f;
			LinePoint startPillar = new LinePoint(end.X, bottomOfHole, 6f, 3f);
			LinePoint middlePillar = new LinePoint(end.X, bottomOfHole - pillarHeight * 0.5f, 1f, 1.4f);
			LinePoint endPillar = new LinePoint(end.X, bottomOfHole - pillarHeight, 3f, 0f);
			RadiusLine pillar = new RadiusLine(0.15f, startPillar, middlePillar, endPillar);
			pillar.Place(_noise, TileID.Dirt);

			//place blood blossom
			int bbX = (int)end.X;
			int testY = (int)endPillar.Point.Y;
			Tile testTile = Framing.GetTileSafely(bbX, testY);
			while(testTile.active()) {
				testY--;
				testTile = Framing.GetTileSafely(bbX, testY);
			}
			ushort bbossom = (ushort)ModContent.TileType<Tiles.BloodBlossom>();
			int fx = 0;
			for(int x = bbX - 1; x <= bbX + 1; x++) {
				//place tile below
				Tile t = Framing.GetTileSafely(x, testY + 1);
				t.active(true);
				t.type = (ushort)ModContent.TileType<Tiles.Block.BriarGrass>();
				t.slope(0);

				int fy = 0;
				//place blood blossom itself
				for(int y = testY - 1; y <= testY; y++) {
					Tile t2 = Framing.GetTileSafely(x, y);
					t2.active(true);
					t2.type = bbossom;
					t2.frameX = (short)(fx * 18);
					t2.frameY = (short)(fy * 18);
					fy++;
				}

				fx++;
			}

			//place floran ore
			//FUCKIN
			//FLOWER ORE
			int veins = (int)(_size.X * _size.Y * 0.00045f); //vein area
			for(int i = 0; i < veins; i++) {
				int x = WorldGen.genRand.Next(_x - _halfSize.X, _x + _halfSize.X);
				int y = WorldGen.genRand.Next(_y + _halfSize.Y / 3, _y + _halfSize.Y);
				if(!WithinEllipse(x, y, _center, _size)) {
					i--;
					continue;
				}
				DoFlowerOre(x, y, WorldGen.genRand.Next(1, 4) * 2 + 1, WorldGen.genRand.Next(4, 11));
			}
		}

		private void DoFlowerOre(int x, int y, int k, int size = 10)
		{
			float myOffset = WorldGen.genRand.NextFloat(MathHelper.TwoPi);
			int halfSize = size / 2;
			for(int oreX = x - halfSize; oreX <= x + halfSize; oreX++) {
				for(int oreY = y - halfSize; oreY <= y + halfSize; oreY++) {
					if(WithinRoseCurve(k, x, y, oreX, oreY, size, myOffset)) {
						Tile tile = Framing.GetTileSafely(oreX, oreY);
						if(tile.active() && (tile.type == TileID.Stone || tile.type == TileID.Dirt)) {
							tile.type = (ushort)ModContent.TileType<Tiles.Block.FloranOreTile>();
						}
					}
				}
			}
			WorldGen.OreRunner(x, y, 3, 2, (ushort)ModContent.TileType<Tiles.Block.FloranOreTile>());
		}

		private bool WithinRoseCurve(int k, int originX, int originY, int x, int y, int size, float offset)
		{
			//convert tile to polar coord
			Vector2 tilePoint = new Vector2(x, y);
			Vector2 center = new Vector2(originX, originY);
			Vector2 off = tilePoint - center;
			off /= size;
			Vector2 normed = Vector2.Normalize(off);
			float angle = (float)Math.Atan2(normed.Y, normed.X) + offset;
			return off.Length() < RoseCurveValue(k, angle + offset);
		}

		private float RoseCurveValue(int k, float angle)
		{
			return (float)Math.Cos(k * angle);
		}

		private void TryPopulate(int amount, ushort type, short frameX, short frameY, int width, int height, ushort expectedTile, Action<ushort, int, int, short, short, int, int, ushort> test)
		{
			for(int i = 0; i < amount; i++) {
				int x = WorldGen.genRand.Next(_x - _halfSize.X, _x + _halfSize.X);
				int y = WorldGen.genRand.Next(_y - _topSize.Y, _y + _halfSize.Y);
				bool valid = y < _y ? WithinEllipse(x, y, _center, _topSize) : WithinEllipse(x, y, _center, _size);
				if(!valid) {
					continue;
				}

				test?.Invoke(type, x, y, frameX, frameY, width, height, expectedTile);
			}
		}

		private void DefaultPopulateTest(ushort type, int x, int y, short frameXStart, short frameYStart, int width, int height, ushort expectedFloorTile = 10000)
		{
			//assume bottom left
			//check solids below
			Tile test = Framing.GetTileSafely(x, y);
			if(test.active()) return;

			for(int testX = x; testX < x + width; testX++) {
				test = Framing.GetTileSafely(testX, y + 1);
				if(!test.active() || !Main.tileSolid[test.type]) return;
				if(expectedFloorTile < 10000 && test.type != expectedFloorTile) return;

				for(int testY = y - height + 1; testY <= y; testY++) {
					test = Framing.GetTileSafely(testX, testY);
					if(test.active()) return;
				}
			}

			//place
			int frameX = 0;
			if(type == TileID.Lamps) frameX = 1; //just a temp thing to turn lamps off
			for(int testX = x; testX < x + width; testX++) {
				int frameY = 0;
				for(int testY = y - height + 1; testY <= y + 1; testY++) {
					Tile tile = Framing.GetTileSafely(testX, testY);

					if(testY <= y) {
						tile.active(true);
						tile.type = type;
						tile.frameX = (short)(frameXStart + frameX * 18);
						tile.frameY = (short)(frameYStart + frameY * 18);

						frameY++;
					} else {
						tile.slope(0);
					}
				}
				frameX++;
				frameY = 0;
			}
		}

		private bool CheckBuildingValid(int minX, int maxX, int minY, int maxY)
		{
			//check this building is mostly in floor, otherwise don't build
			int total = maxX - minX + 1;
			int count = 0;
			bool opps = false;
			bool left = false;
			for(int i = minX; i <= maxX; i++) {
				Tile tile = Framing.GetTileSafely(i, maxY + 1);
				if(tile.active() && Main.tileSolid[tile.type]) {
					count++;
					if(i < minX / 2 - 2) left = true;
					else if(i > minX / 2 + 2 && left) {
						opps = true;
						break;
					}
				}
			}
			return opps || count > total * 0.6;
		}

		private void CreateBuildingAt(EaseFunction function, int minX, int maxX, int minY, int maxY, ref bool placedAdventurer)
		{
			List<Point> polygonVertices = new List<Point>();

			polygonVertices.Add(new Point(minX, maxY));

			float width = maxX - minX;
			float rand = WorldGen.genRand.NextFloat(1f);
			int heightBonus = (int)((maxY - minY) * 0.33f);
			Vector2 leftControl = new Vector2(minX + function.Ease(rand) * width, minY - heightBonus);
			Vector2 rightControl = new Vector2(minX + function.Ease(Math.Min(1f, rand + 0.2f)) * width, minY - heightBonus);

			BezierCurve curve = new BezierCurve(new Vector2(minX, maxY), leftControl, rightControl, new Vector2(maxX, maxY));
			List<Vector2> points = curve.GetPoints(WorldGen.genRand.Next(4, 7));

			for(int i = 0; i < points.Count; i++) {
				Point p = points[i].ToPoint();
				if(i != 0 && i != points.Count - 1) {
					p.X += WorldGen.genRand.Next(-1, 2);
					p.Y += WorldGen.genRand.Next(-1, 2);
				}
				p.X = ClampInt(p.X, minX, maxX);
				p.Y = ClampInt(p.Y, minY, maxY);
				polygonVertices.Add(p);
			}

			ushort boundaryTile = (ushort)ModContent.TileType<BarkTileTile>(); //TODO: CHANGE
			ushort[] extras = new ushort[] { TileID.LivingMahoganyLeaves, (ushort)ModContent.TileType<BlastStone>() };
			ushort[] wallTypes = new ushort[] { (ushort)ModContent.WallType<BarkWall>(), WallID.LivingLeaf, WallID.MudstoneBrick }; //TODO: CHANGE

			//place shape
			for(int i = 0; i < polygonVertices.Count; i++) {
				Point start = polygonVertices[i];
				Point end = i == polygonVertices.Count - 1 ? polygonVertices[0] : polygonVertices[i + 1];

				ConnectedBresenhamPlace(start.X, start.Y, end.X, end.Y, boundaryTile);
			}

			Dictionary<int, Point> interiorSections = new Dictionary<int, Point>();

			bool BreakInnerLoop(int x, int y, ref int ct)
			{
				Tile tile = Framing.GetTileSafely(x, y);
				if(tile.active() && tile.type == boundaryTile) {
					ct++;
				} else if(ct > 0) {
					return true;
				}
				return false;
			}

			//find all "inner" tiles
			for(int y = minY; y <= maxY; y++) {
				int ct = 0;
				int leftX = minX;
				for(; leftX <= maxX; leftX++) {
					if(BreakInnerLoop(leftX, y, ref ct)) break;
				}
				if(ct < 1) continue;
				int rightX = maxX;
				ct = 0;
				for(; rightX >= minX; rightX--) {
					if(BreakInnerLoop(rightX, y, ref ct)) break;
				}
				if(leftX <= rightX) {
					interiorSections[y] = new Point(leftX, rightX);
					for(int x = leftX; x <= rightX; x++) {
						Tile tile = Framing.GetTileSafely(x, y);
						tile.active(false);
					}
				}
			}

			//this function only checks for air inside
			bool WithinInterior(int x, int y)
			{
				if(interiorSections.TryGetValue(y, out Point bounds)) {
					return x >= bounds.X && x <= bounds.Y;
				}
				return false;
			}

			//this function keeps one block of wall as well
			bool BuildingOwned(int x, int y)
			{
				Tile tile = Framing.GetTileSafely(x, y);
				if(tile.active() && tile.type == boundaryTile) return true;
				return WithinInterior(x, y);
			}

			int strips = WorldGen.genRand.Next(7, 12);
			for(int i = 0; i < strips; i++) {
				(Point, Point) line = SelectRandomLineInRect(minX, minY, maxX, maxY);
				Vector2 start = line.Item1.ToVector2();
				Vector2 end = line.Item2.ToVector2();
				Vector2 toMid = (end - start) * 0.5f;
				Vector2 mid = start + toMid.RotatedByRandom(0.8);
				BezierCurve stripCurve = new BezierCurve(start, mid, end);
				int pointCt = WorldGen.genRand.Next(3, 7);
				List<Vector2> stripPoints = stripCurve.GetPoints(pointCt);
				for(int j = 1; j < pointCt; j++) {
					if(i >= 2 && i <= pointCt - 2 && WorldGen.genRand.Next(4) == 0) continue;

					Point sStart = stripPoints[j - 1].ToPoint();
					Point sEnd = stripPoints[j].ToPoint();
					ConnectedBresenhamPlace(sStart.X, sStart.Y, sEnd.X, sEnd.Y, WorldGen.genRand.Next(wallTypes), true, WithinInterior);
				}
			}

			//place leaves / muddy brick shit
			int leafAttempts = WorldGen.genRand.Next(10, 15);
			for(int i = 0; i < leafAttempts; i++) {
				int x = WorldGen.genRand.Next(minX, maxX + 1);
				int y = WorldGen.genRand.Next(minY, maxY + 1);
				if(!WithinInterior(x, y)) {
					i--;
					continue;
				}

				CopiedFromSourceBasicRunner(x, y, WorldGen.genRand.Next(2, 4), WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(extras), WithinInterior, BuildingOwned);
			}

			//place entrances

			//if true, place roof entrance
			bool doTop = WorldGen.genRand.NextBool();
			bool topActuallyPlaced = BuildingAttemptTopEntrance(boundaryTile, extras, minX, maxX, minY, maxY, 20);

			//try put bottom platforms in
			bool doBottom = !doTop ? true : WorldGen.genRand.NextBool();
			bool bottomActuallyPlaced = false;
			if(doBottom) {
				int attempts = 0;
				while(attempts < 80) {
					int x = WorldGen.genRand.Next(minX + 1, maxX - 2);
					bool failed = false;
					for(int testX = x; testX <= x + 1; testX++) {
						for(int testY = maxY - 3; testY <= maxY - 1; testY++) {
							if(Framing.GetTileSafely(testX, testY).active()) {
								failed = true;
								break;
							}
						}
						if(failed) break;

						Tile tile = Framing.GetTileSafely(testX, maxY + 1);
						if(tile.active()) {
							failed = true;
							break;
						}
					}
					attempts++;
					if(failed) continue;

					Tile platform1 = Framing.GetTileSafely(x, maxY);
					Tile platform2 = Framing.GetTileSafely(x + 1, maxY);

					platform1.type = platform2.type = TileID.Platforms;
					platform1.frameY = platform2.frameY = 0;
					bottomActuallyPlaced = true;

					WorldGen.SquareTileFrame(x, maxY);
					WorldGen.SquareTileFrame(x + 1, maxY);

					break;
				}
			}

			if(!topActuallyPlaced && !bottomActuallyPlaced) {
				//PANIC, try place top again but with more errors available
				BuildingAttemptTopEntrance(boundaryTile, extras, minX, maxX, minY, maxY, 60);
			}

			//type, width, height, frameX, frameY
			List<Func<(ushort, int, int, int, int)>> furnituresAndTiles = GetFurnitureList();
			//add statues

			int furnitureX = minX;
			while(furnitureX < maxX) {
				if(furnituresAndTiles.Count == 0) break;

				furnitureX++;
				if(!WithinInterior(furnitureX, maxY - 1)) continue;

				if(WorldGen.genRand.Next(3) == 0) {
					int index = WorldGen.genRand.Next(furnituresAndTiles.Count);
					(ushort, int, int, int, int) data = furnituresAndTiles[index]();

					bool failed = false;
					for(int testX = furnitureX; testX < furnitureX + data.Item2; testX++) {
						for(int testY = maxY - data.Item3; testY < maxY; testY++) {
							Tile tile = Framing.GetTileSafely(testX, testY);
							if(tile.active()) {
								failed = true;
								break;
							}
						}
						if(failed) break;
					}
					if(failed) continue;

					//place it
					short fX = (short)data.Item4;
					short fY = (short)data.Item5;
					for(int tileX = furnitureX; tileX < furnitureX + data.Item2; tileX++) {
						for(int tileY = maxY - data.Item3; tileY < maxY; tileY++) {
							Tile tile = Framing.GetTileSafely(tileX, tileY);
							tile.active(true);
							tile.type = data.Item1;
							tile.frameX = fX;
							tile.frameY = fY;
							fY += 18;
						}
						fX += 18;
						fY = (short)data.Item5;
					}

					if(data.Item1 == ModContent.TileType<Tiles.Furniture.ReachChest>()) //TODO: Make specific chest
					{
						int cIndex = Chest.CreateChest(furnitureX, maxY - 2);
					}

					furnitureX += data.Item2 - 1;

					furnituresAndTiles.RemoveAt(index);
				}
			}

			if(!placedAdventurer) {
				for(int x = minX + 1; x < maxX - 2; x++) {
					placedAdventurer = TryPlaceAdventurer(x, maxY - 1);

					if(placedAdventurer) break;
				}
			}
		}

		private bool TryPlaceAdventurer(int x, int y)
		{
			for(int testX = x; testX < x + 3; testX++) {
				Tile test = Framing.GetTileSafely(testX, y + 1);
				if(!test.active()) return false;

				for(int testY = y - 3 + 1; testY <= y; testY++) {
					test = Framing.GetTileSafely(testX, testY);
					if(test.active()) return false;
				}
			}

			int adv = NPC.NewNPC((x + 1) * 16 + 8, (y - 1) * 16 + 8, ModContent.NPCType<NPCs.Town.BoundAdventurer>());
			Main.npc[adv].homeTileX = -1;
			Main.npc[adv].homeTileY = -1;
			Main.npc[adv].direction = WorldGen.genRand.NextBool() ? 1 : -1;
			Main.npc[adv].homeless = true;

			return true;
		}

		private List<Func<(ushort, int, int, int, int)>> GetFurnitureList()
		{
			List<Func<(ushort, int, int, int, int)>> furnituresAndTiles = new List<Func<(ushort, int, int, int, int)>>();
			furnituresAndTiles.Add(() => {
				return (TileID.Statues, 2, 3, 36 * WorldGen.genRand.Next(43), 0);
			});
			furnituresAndTiles.Add(() => ((ushort)ModContent.TileType<Tiles.Furniture.ReachChest>(), 2, 2, 0, 0));  //TODO: make specific chest
			furnituresAndTiles.Add(() => ((ushort)ModContent.TileType<Tiles.Furniture.BoneAltar>(), 3, 3, 0, 0));  //TODO: make specific chest
			return furnituresAndTiles;
		}

		private bool BuildingAttemptTopEntrance(ushort boundaryTile, ushort[] extras, int minX, int maxX, int minY, int maxY, int attemptMax)
		{
			int attempts = 0;
			while(attempts < attemptMax) {
				attempts++;

				int xTest = WorldGen.genRand.Next(minX + 1, maxX - 1);
				int y = maxY - 1;
				int space = 0;
				bool failed = false;
				while(y >= minY) {
					Tile tile = Framing.GetTileSafely(xTest, y);
					Tile tile2 = Framing.GetTileSafely(xTest + 1, y);
					if(tile.active() || tile2.active()) {
						if(space < 3) {
							break;
						}

						if(tile.active() && tile2.active()) {
							List<Point> removals = new List<Point>();
							for(int sX = xTest; sX <= xTest + 1; sX++) {
								for(int sY = y - 3; sY <= y + 2; sY++) {
									Tile sTile = Framing.GetTileSafely(sX, sY);
									if(sTile.active()) {
										if(sTile.type != boundaryTile && !extras.Contains(sTile.type)) {
											failed = true;
											break;
										} else {
											removals.Add(new Point(sX, sY));
										}
									}
								}
								if(failed) break;
							}
							if(Framing.GetTileSafely(xTest, y - 4).active() || Framing.GetTileSafely(xTest + 1, y - 4).active()) {
								failed = true;
							}
							if(failed) break;

							foreach(Point p in removals) {
								Framing.GetTileSafely(p.X, p.Y).active(false);
							}
							tile.active(true);
							tile2.active(true);
							tile.type = tile2.type = TileID.Platforms;
							tile.frameY = tile2.frameY = 0;
							WorldGen.SquareTileFrame(xTest, y);
							WorldGen.SquareTileFrame(xTest + 1, y);
							Tile tile3 = Framing.GetTileSafely(xTest - 1, y);
							Tile tile4 = Framing.GetTileSafely(xTest + 2, y);
							tile3.active(true);
							tile4.active(true);
							tile3.type = boundaryTile;
							tile4.type = boundaryTile;

							return true;
						}
					}
					space++;
					y--;
				}
			}
			return false;
		}

		private void CopiedFromSourceBasicRunner(int i, int j, int steps, int strength, ushort type, Func<int, int, bool> baseCheck, Func<int, int, bool> invalidCheck)
		{
			Vector2 vector2 = new Vector2();
			Vector2 vector21 = new Vector2();
			double num = strength;
			double num1 = num;
			float single = steps;
			vector2.X = (float)i;
			vector2.Y = (float)j - single * 0.3f;
			vector21.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			vector21.Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			while(num > 0 && single > 0f) {
				if((double)vector2.Y + num1 * 0.5 > Main.worldSurface) {
					single = 0f;
				}
				num -= (double)WorldGen.genRand.Next(3);
				single -= 1f;
				int x = (int)((double)vector2.X - num * 0.5);
				int x1 = (int)((double)vector2.X + num * 0.5);
				int y = (int)((double)vector2.Y - num * 0.5);
				int y1 = (int)((double)vector2.Y + num * 0.5);
				if(x < 0) {
					x = 0;
				}
				if(x1 > Main.maxTilesX) {
					x1 = Main.maxTilesX;
				}
				if(y < 0) {
					y = 0;
				}
				if(y1 > Main.maxTilesY) {
					y1 = Main.maxTilesY;
				}
				num1 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
				int midX = (int)(x + (x1 - x) * 0.5f);
				int midY = (int)(y + (y1 - y) * 0.5f);
				if(baseCheck(midX, midY)) {
					for(int i1 = x; i1 < x1; i1++) {
						for(int j1 = y; j1 < y1; j1++) {
							if(invalidCheck(i1, j1)) continue;

							float single1 = Math.Abs((float)i1 - vector2.X);
							float single2 = Math.Abs((float)j1 - vector2.Y);
							if(Math.Sqrt((double)(single1 * single1 + single2 * single2)) < num1 * 0.6) {
								Tile tile = Framing.GetTileSafely(i1, j1);
								tile.active(true);
								tile.type = type;
							}
						}
					}
				}
				vector2 += vector21;
				vector21.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				vector21.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				if((double)vector21.X > 0.5) {
					vector21.X = 0.5f;
				}
				if((double)vector21.X < -0.5) {
					vector21.X = -0.5f;
				}
				if((double)vector21.Y > 1.5) {
					vector21.Y = 1.5f;
				}
				if((double)vector21.Y >= 0.5) {
					continue;
				}
				vector21.Y = 0.5f;
			}
		}

		private (Point, Point) SelectRandomLineInRect(int minX, int minY, int maxX, int maxY)
		{
			bool roofWalls = WorldGen.genRand.NextBool();
			bool topOrLeft = WorldGen.genRand.NextBool();
			Point p1 = SelectPointOnRectEdge(minX, minY, maxX, maxY, roofWalls, topOrLeft);
			Point p2 = SelectPointOnRectEdge(minX, minY, maxX, maxY, roofWalls, !topOrLeft);
			return (p1, p2);
		}

		private Point SelectPointOnRectEdge(int minX, int minY, int maxX, int maxY, bool roofWalls, bool topOrLeft)
		{
			int x, y;
			if(roofWalls) {
				y = topOrLeft ? minY : maxY;
				x = WorldGen.genRand.Next(minX, maxX + 1);
			} else {
				x = topOrLeft ? minX : maxX;
				y = WorldGen.genRand.Next(minY, maxY + 1);
			}
			return new Point(x, y);
		}

		private int ClampInt(int value, int min, int max)
		{
			return value < min ? min : (value > max ? max : value);
		}

		/// <summary>
		/// Draw a line of tiles (or walls) from x0, y0 to x1, y1 which are connected (diagonals have a tile connection)
		/// </summary>
		private void ConnectedBresenhamPlace(int x0, int y0, int x1, int y1, ushort type, bool wall = false, Func<int, int, bool> check = null)
		{
			int distX = Math.Abs(x1 - x0);
			int distY = Math.Abs(y1 - y0);

			int ix = (x0 < x1) ? 1 : -1;
			int iy = (y0 < y1) ? 1 : -1;

			int e = 0;

			for(int i = 0; i < distX + distY; i++) {
				if(check == null || check(x0, y0)) {
					Tile t = Framing.GetTileSafely(x0, y0);
					if(wall) {
						t.wall = type;
					} else {
						t.active(true);
						t.slope(0);
						t.type = type;
					}
				}

				int e1 = e + distY;
				int e2 = e - distX;
				if(Math.Abs(e1) < Math.Abs(e2)) {
					x0 += ix;
					e = e1;
				} else {
					y0 += iy;
					e = e2;
				}
			}
		}

		private bool SafeCheck(int tileX, List<ushort> surfaceIds, List<ushort> underIds)
		{
			int midWorld = Main.maxTilesX / 2;
			int minXUnsafeSpawn = midWorld - _halfSize.X - 100;
			int maxXUnsafeSpawn = midWorld + _halfSize.X + 100;
			if(tileX > minXUnsafeSpawn && tileX < maxXUnsafeSpawn) return false;

			int dungeonMinUnsafe = Main.dungeonX - 150;
			int dungeonMaxUnsafe = Main.dungeonX + 150;
			if(tileX > dungeonMinUnsafe && tileX < dungeonMaxUnsafe) return false;

			int y = FindSurfaceAt(tileX);
			int spacing = 7;
			for(int yTest = y; yTest < y + spacing * 10; yTest += spacing) //checks 70 blocks below surface (too high?) 
			{
				Tile tile = Framing.GetTileSafely(tileX, yTest);
				if((yTest == y && IsOneOf(tile, surfaceIds)) || (yTest != y && IsOneOf(tile, underIds))) {
					return false;
				}
			}
			return true;
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int index = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle Chests"));
			if(index == -1) {
				return;
			}

			List<ushort> BadSurfaceIDs = new List<ushort>() { TileID.SnowBlock, TileID.Ebonstone, TileID.Crimstone, TileID.FleshGrass, TileID.CorruptGrass, TileID.IceBlock, TileID.HardenedSand, TileID.Sandstone, TileID.HardenedSand, TileID.JungleGrass };
			List<ushort> BadUndergroundIDs = new List<ushort>() { TileID.Ebonstone, TileID.Crimstone, TileID.FleshGrass, TileID.CorruptGrass, TileID.IceBlock, TileID.HardenedSand, TileID.Sandstone, TileID.HardenedSand, TileID.JungleGrass };

			int worldSizeBonus = (int)((Main.maxTilesX / 4200f) * 8f);
			int minX = 500;
			int maxX = Main.maxTilesX - 500;
			_size = new Point(250, 600);
			if(Main.maxTilesX == 6400) {
				_size = new Point(350, 780);
			} else if(Main.maxTilesX > 6400) {
				_size = new Point(460, 940);
			}
			_halfSize = new Point(_size.X / 2, _size.Y / 2);

			tasks.Insert(++index, new PassLegacy("Briar",
				delegate (GenerationProgress progress) {
					progress.Message = "Growing The Briar...";

					int tilePer = 10;
					int safeInARow = 0;
					int highestInARow = 0;
					int prevSurfaceY = -1;
					for(int tileX = minX; tileX <= maxX; tileX += tilePer) {
						int surfaceY = FindSurfaceAt(tileX);
						bool safe = true;
						if(prevSurfaceY != -1 && Math.Abs(surfaceY - prevSurfaceY) > 10) safe = false;
						if(safe) {
							safe = SafeCheck(tileX, BadSurfaceIDs, BadUndergroundIDs);
						}
						if(safe) {
							safeInARow++;
							if(safeInARow > highestInARow) {
								highestInARow = safeInARow;
								if(highestInARow % 2 == 1) {
									_x = tileX - tilePer * (int)Math.Ceiling(highestInARow / 2.0);
								}
							}
						} else {
							safeInARow = 0;
						}
						prevSurfaceY = surfaceY;
					}

					_y = FindSurfaceAt(_x);
					PlaceBriar();
				}, 300f));

			tasks.Insert(index + 2, new PassLegacy("Briar Grass",
				delegate (GenerationProgress progress) {
					progress.Message = "Growing Hostile Settlements...";

					// BLOCK TYPES
					ushort briarGrass = (ushort)ModContent.TileType<Tiles.Block.BriarGrass>();

					ushort briarStoneWall = (ushort)ModContent.WallType<Tiles.Walls.Natural.ReachStoneWall>();
					ushort briarNaturalWall = (ushort)ModContent.WallType<Tiles.Walls.Natural.ReachWallNatural>();
					ushort mudWall = WallID.MudUnsafe;
					//

					List<ushort> ignoreWalls = new List<ushort>()
					{
						WallID.CrimsonUnsafe1,
						WallID.CrimsonUnsafe2,
						WallID.CrimsonUnsafe3,
						WallID.CrimsonUnsafe4,
						WallID.CorruptionUnsafe1,
						WallID.CorruptionUnsafe2,
						WallID.CorruptionUnsafe3,
						WallID.CorruptionUnsafe4,
						WallID.Sandstone,
						WallID.HiveUnsafe
					};

					int yTop = (int)(_y - _topSize.Y * 0.5f);
					int yBottom = _y + _halfSize.Y;
					float yTotal = yBottom - yTop;
					float noiseScale = 0.11f;

					int hMinX = (int)(_houseCenter.X - _houseGrassRadius);
					int hMaxX = (int)(_houseCenter.X + _houseGrassRadius);
					int hMinY = (int)(_houseCenter.Y - _houseGrassRadius);
					int hMaxY = (int)(_houseCenter.Y + _houseGrassRadius);

					//Final grass pass and walls
					for(int tileX = _x - _halfSize.X; tileX <= _x + _halfSize.X; tileX++) {
						for(int tileY = yTop; tileY <= yBottom; tileY++) {
							bool valid = tileY < Main.worldSurface;

							if(tileY <= _y) valid |= WithinEllipseNoise(tileX, tileY, _center, _topSize, _estimates, _noise);
							else valid |= WithinEllipseNoise(tileX, tileY, _center, _size, _estimates, _noise);

							if(valid) {
								TryPlaceGrass(tileX, tileY, briarGrass);

								Tile tile = Framing.GetTileSafely(tileX, tileY);
								if(tile.wall != 0 && !ignoreWalls.Contains(tile.wall)) {
									float yProgress = (tileY - yTop) / yTotal; //value between 0f and 1f
									float wallTypeValue = yProgress + (_noise.Noise(tileX * noiseScale, tileY * noiseScale) + 1f) * 0.5f;
									float doWall = yProgress + (_noise.Noise(tileY * -noiseScale, tileX * noiseScale) + 1f) * 0.5f;
									if(doWall < 1.3f) {
										if(wallTypeValue < 1f) {
											//near the surface, grass
											tile.wall = briarNaturalWall;
										} else {
											//nearer the bottom, stone
											tile.wall = mudWall;
										}
									} else {
										tile.wall = (tileY > Main.worldSurface + 20) ? (ushort)0 : WallID.DirtUnsafe;
									}
								}
							}
						}
					}

					//house grass and walls
					for(int tileX = hMinX; tileX <= hMaxX; tileX++) {
						float dX = tileX - _houseCenter.X;
						for(int tileY = hMinY; tileY <= hMaxY; tileY++) {
							float dY = tileY - _houseCenter.Y;
							double dist = Math.Sqrt(dX * dX + dY * dY);
							if(dist < _houseGrassRadius) {
								TryPlaceGrass(tileX, tileY, briarGrass);

								Tile tile = Framing.GetTileSafely(tileX, tileY);
								if(dist < _houseGrassRadius * 0.95f) {
									tile.wall = briarNaturalWall;
								}
							}
						}
					}

					//Further cleaning
					for(int tileX = _x - _halfSize.X; tileX <= _x + _halfSize.X; tileX++) {
						for(int tileY = (int)(_y - _topSize.Y * 0.5f); tileY <= _y + _halfSize.Y; tileY++) {
							bool valid = tileY < Main.worldSurface;

							if(tileY <= _y) valid |= WithinEllipseNoise(tileX, tileY, _center, _topSize, _estimates, _noise);
							else valid |= WithinEllipseNoise(tileX, tileY, _center, _size, _estimates, _noise);

							if(valid) {
								Tile tile = Framing.GetTileSafely(tileX, tileY);
								int neighbours = 0;
								for(int grassX = tileX - 1; grassX <= tileX + 1; grassX++) {
									for(int grassY = tileY - 1; grassY <= tileY + 1; grassY++) {
										Tile test = Framing.GetTileSafely(grassX, grassY);
										if(test.active()) {
											neighbours++;
										}
									}
								}
								if(tile.active() && neighbours < 3) {
									tile.active(false);
								}
							}
						}
					}

					//BUILDINGS
					EaseBuilder bezierPinchEase = new EaseBuilder();
					bezierPinchEase.AddPoint(0.25f, 0f, EaseFunction.Linear);
					bezierPinchEase.AddPoint(0.5f, 0.5f, EaseFunction.EaseQuadInOut);
					bezierPinchEase.AddPoint(0.75f, 0.5f, EaseFunction.Linear);
					bezierPinchEase.AddPoint(1f, 1f, EaseFunction.EaseQuadInOut);

					int minBuildingX = _x - (int)(M_CAVE_RADIUS_TOP * 4);
					int maxBuildingX = _x + (int)(M_CAVE_RADIUS_TOP * 4);
					int minBuildingY = (int)(_caveTop.Y + M_CAVE_RADIUS_TOP);
					int maxBuildingY = (int)(_caveBottom.Y - 50);

					int buildings = WorldGen.genRand.Next((int)(worldSizeBonus * 0.8), (int)(worldSizeBonus * 1.2));
					List<Rectangle> buildingPositions = new List<Rectangle>();

					int errorMax = 2000;
					for(int i = 0; i < buildings; i++) {
						int errors = 0;
						while(errors < errorMax) {
							errors++;
							int x = WorldGen.genRand.Next(minBuildingX, maxBuildingX);
							int y = WorldGen.genRand.Next(minBuildingY, maxBuildingY);

							Tile tile = Framing.GetTileSafely(x, y);
							if(tile.active()) {
								int airSpaces = 0;
								for(int x1 = x - 2; x1 <= x + 2; x1++) {
									for(int y1 = y - 2; y1 <= y; y1++) {
										if(x1 == x && y1 == y) continue;

										Tile tile2 = Framing.GetTileSafely(x1, y1);
										if(!tile2.active()) {
											airSpaces++;
										}
									}
								}

								if(airSpaces > 8) {
									bool valid = true;
									Vector2 myPos = new Vector2(x, y);
									foreach(Rectangle pos in buildingPositions) {
										if(Vector2.Distance(myPos, pos.Center()) < 40f) {
											valid = false;
											break;
										}
									}
									if(!valid) continue;

									Point center = myPos.ToPoint();
									int width = WorldGen.genRand.Next(10, 16);
									int height = WorldGen.genRand.Next(8, 10);
									int minBX = center.X - width / 2;
									int minBY = center.Y - height / 2;
									int maxBX = minBX + width;
									int maxBY = minBY + height;
									if(CheckBuildingValid(minBX, maxBX, minBY, maxBY)) {
										buildingPositions.Add(new Rectangle(minBX, minBY, width, height));
										break;
									}
								}
							}
						}
					}

					bool placedAdventurer = false;
					foreach(Rectangle r in buildingPositions) {
						CreateBuildingAt(bezierPinchEase, r.X, r.X + r.Width, r.Y, r.Y + r.Height, ref placedAdventurer);
					}

					int error = 0;
					while(!placedAdventurer && error < 10000) {
						int x = WorldGen.genRand.Next(_x - _halfSize.X, _x + _halfSize.X);
						int y = WorldGen.genRand.Next(_y + _halfSize.Y / 3, _y + _halfSize.Y);
						if(WithinEllipse(x, y, _center, _size)) {
							placedAdventurer = TryPlaceAdventurer(x, y);
						}
						error++;
					}

					int treeAttempts = 0;
					int treesPlaced = 0;
					while(treeAttempts < 10) {
						treeAttempts++;

						int wiggleTreeX = WorldGen.genRand.Next(_x - _halfSize.X, _x + _halfSize.X);
						int wiggleY = FindSurfaceAt(wiggleTreeX);
						bool failed = false;
						for(int testX = wiggleTreeX - 20; testX <= wiggleTreeX + 20; testX++) {
							Tile test1 = Framing.GetTileSafely(testX, wiggleY);
							Tile test2 = Framing.GetTileSafely(testX, wiggleY - 20);
							if((test1.active() && test1.type == TileID.LeafBlock) || (test2.active() && test2.type == TileID.LeafBlock)) {
								failed = true;
								continue;
							}
						}
						if(failed) continue;

						GrowWigglyTree(wiggleTreeX, wiggleY, treesPlaced == 0 ? 5 : 2, treesPlaced == 0 ? 60 : 24);
						treesPlaced += WorldGen.genRand.Next(1, 3);
						if(treesPlaced >= 2) {
							break;
						}
					}
					//trees
					for(int tileX = _x - _halfSize.X; tileX <= _x + _halfSize.X; tileX++) {
						for(int tileY = _y - _topSize.Y; tileY <= _y + _halfSize.Y; tileY++) {
							if(tileY < _y + 20) {
								if(WorldGen.genRand.Next(2) == 0) {
									GrowTreeCustom(tileX, tileY, 16, 32);
								}
							} else {
								GrowTreeCustom(tileX, tileY, 5, 10);
							}
						}
					}

					TryPopulate(2000, (ushort)ModContent.TileType<Tiles.Ambient.SkullStick>(), 0, 0, 2, 4, 10000, DefaultPopulateTest);

					//both directions of big flower
					TryPopulate(4000, (ushort)ModContent.TileType<Tiles.Ambient.Briar.BriarBigFlower>(), 0, 0, 3, 3, briarGrass, DefaultPopulateTest);
					TryPopulate(4000, (ushort)ModContent.TileType<Tiles.Ambient.Briar.BriarBigFlower>(), 54, 0, 3, 3, briarGrass, DefaultPopulateTest);

					//Vines and foliage
					ushort vineType = (ushort)ModContent.TileType<Tiles.Ambient.Briar.BriarVines>();
					ushort foliageType = (ushort)ModContent.TileType<Tiles.Ambient.Briar.BriarFoliage>();
					for(int tileX = _x - _halfSize.X; tileX <= _x + _halfSize.X; tileX++) {
						int vineLeft = 0;
						int tileY = _y - _topSize.Y;
						while(tileY < _y + _halfSize.Y) {
							DoGrassFoliage(tileX, ref tileY, ref vineLeft, vineType, briarGrass, foliageType);
						}
					}
					//house foliage
					for(int tileX = hMinX; tileX <= hMaxX; tileX++) {
						float dX = tileX - _houseCenter.X;
						for(int tileY = hMinY; tileY <= hMaxY; tileY++) {
							float dY = tileY - _houseCenter.Y;
							int vineLeft = 0;
							if(Math.Sqrt(dX * dX + dY * dY) < _houseGrassRadius) {
								DoGrassFoliage(tileX, ref tileY, ref vineLeft, vineType, briarGrass, foliageType);
							}
						}
					}

					//wWriteTextInWorld(Main.worldName, TileID.DiamondGemspark, 100, 100, Main.fontMouseText, 4);

				}, 100f));
		}

		private void TryPlaceGrass(int tileX, int tileY, ushort briarGrass)
		{
			Tile tile = Framing.GetTileSafely(tileX, tileY);
			int neighbours = 0;
			for(int grassX = tileX - 1; grassX <= tileX + 1; grassX++) {
				for(int grassY = tileY - 1; grassY <= tileY + 1; grassY++) {
					Tile test = Framing.GetTileSafely(grassX, grassY);
					if(!test.active()) {
						if(tile.type == TileID.Dirt || TileID.Sets.Grass[tile.type]) {
							tile.type = briarGrass;
						}
						if(tile.active() && test.wall == 0) {
							tile.wall = 0;
						}
					} else {
						neighbours++;
					}
				}
			}
			if(tile.active() && neighbours < 3) {
				tile.active(false);
				tile.wall = 0; //TODO: Working?
			}
		}

		private void DoGrassFoliage(int tileX, ref int tileY, ref int vineLeft, ushort vineType, ushort briarGrass, ushort foliageType)
		{
			Tile tile = Framing.GetTileSafely(tileX, tileY);
			Tile tileBelow = Framing.GetTileSafely(tileX, tileY + 1);

			//try vines
			if(vineLeft > 0 && !tile.active()) {
				tile.active(true);
				tile.type = vineType;
				vineLeft--;
			} else {
				vineLeft = 0;
			}
			if(tile.active() && !tile.bottomSlope() && tile.type == briarGrass && WorldGen.genRand.NextBool(3, 5)) {
				vineLeft = WorldGen.genRand.Next(1, 10);
			}
			tileY++;

			//try foliage
			if(WorldGen.genRand.NextBool(9, 10) && !tile.active() && tileBelow.active() && !tileBelow.topSlope() && !tileBelow.halfBrick() && tileBelow.type == briarGrass) {
				tile.active(true);
				tile.type = foliageType;
				tile.frameY = 0;
				tile.frameX = (short)(18 * WorldGen.genRand.Next(8));
				//WorldGen.Place1x1(tileX, tileY, foliageType, WorldGen.genRand.Next(8));
			}
		}

		/*private void WriteTextInWorld(string text, ushort tileType, int x, int y, DynamicSpriteFont font, int scale = 1)
		{
			Type characterData = typeof(DynamicSpriteFont).GetNestedType("SpriteCharacterData", BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo getCharData = typeof(DynamicSpriteFont).GetMethod("GetCharacterData", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo characterDataTexture = characterData.GetField("Texture", BindingFlags.Public | BindingFlags.Instance);
			FieldInfo characterDataGlyph = characterData.GetField("writGlyph", BindingFlags.Public | BindingFlags.Instance);

			int myX = x;
			int myY = y;

			object charData = getCharData.Invoke(font, new object[] { 'a' });
			Texture2D glyphTexture = (Texture2D)characterDataTexture.GetValue(charData);
			byte[] data = new byte[glyphTexture.Width * glyphTexture.Height];
			glyphTexture.GetData(data);
			byte[,] alphaData = GetAlphaData(data, glyphTexture.Width, glyphTexture.Height);

			foreach (char c in text)
			{
				Rectangle area = (Rectangle)characterDataGlyph.GetValue(getCharData.Invoke(font, new object[] { c }));

				for (int i = 0; i < area.Width; i++)
				{
					for (int j = 0; j < area.Height; j++)
					{
						byte alpha = alphaData[area.X + i, area.Y + j];
						if (alpha > 2)
						{
							for (int a = 0; a < scale; a++)
							{
								for (int b = 0; b < scale; b++)
								{
									Tile tile = Framing.GetTileSafely(myX + i * scale + a, myY + j * scale + b);
									tile.active(true);
									tile.type = tileType;
								}
							}
						}
					}
				}

				myX += area.Width * scale + 4;
				if (c == ' ')
				{
					myX += (int)font.MeasureString(" ").X * scale;
				}
			}
		}

		byte[,] GetAlphaData(byte[] dxt3data, int width, int height)
		{
			byte[,] pixelAlpha = new byte[width, height];

			//16 bytes = 8 bytes of alpha data, 8 of color
			//so only every other section of 8 bytes matter
			//8 bytes = 64 bits, 4 bits for alpha per pixel?

			int total = 0;
			int index = 0;
			bool collect = true;
			byte[] alphaData = new byte[dxt3data.Length / 2];
			for (int i = 0; i < dxt3data.Length; i++)
			{
				if (collect)
				{
					alphaData[index++] = dxt3data[i];
				}

				total++;
				if (total > 7)
				{
					collect = !collect;
					total = 0;
				}
			}

			int blockX = 0;
			int blockY = 0;
			int pixelX = 0;
			int pixelY = 0;
			//so now, every 8 bytes represents a consecutive 4x4 block of pixels alpha data, each byte containing 4 bits of 1 pixel (assuming it's l->r t->b?)
			for (int i = 0; i < alphaData.Length; i++)
			{
				int pX = blockX * 4;
				int pY = blockY * 4;
				pixelAlpha[pixelX + pX, pixelY + pY] = (byte)(alphaData[i] & 0b0000_1111); //right side
				pixelAlpha[pixelX + pX + 1, pixelY + pY] = (byte)((alphaData[i] & 0b1111_0000) >> 4); //left side

				pixelX += 2;
				if (pixelX > 3)
				{
					pixelX = 0;
					pixelY++;
					if (pixelY > 3)
					{
						blockX++;
						pixelY = 0;
						if (blockX >= width / 4)
						{
							blockX = 0;
							blockY++;
							if (blockY >= height / 4) break;
						}
					}
				}
			}

			return pixelAlpha;
		}*/

		private void GrowWigglyTree(int x, int y, int halfWidth = 5, int height = 60)
		{
			float startRadius = 3;
			float offset = WorldGen.genRand.NextBool() ? 0 : MathHelper.Pi;
			float offset2 = WorldGen.genRand.NextFloat(MathHelper.TwoPi);
			double wiggleSpeed = (double)WorldGen.genRand.NextFloat(0.2f, 0.5f);
			double wiggleSpeed2 = (double)WorldGen.genRand.NextFloat(0.03f, 0.08f);

			float GenerateXOffset(int yValue)
			{
				return (float)Math.Sin((yValue - y) * wiggleSpeed + offset) * (float)Math.Sin((yValue - y) * wiggleSpeed2 + offset2);
			}

			int topMiddlestXIndex = 0;
			int index = 0;
			int topY = -1;
			List<LinePoint> treePoints = new List<LinePoint>();

			bool add = true;
			for(int testY = y; testY >= y - height; testY--) {
				float progress = 1f - ((y - testY) / (float)height);
				float xOffset = GenerateXOffset(testY);
				if(xOffset > -0.2 && xOffset < 0.2) {
					topMiddlestXIndex = index;
					topY = testY;
				}
				if(add) {
					treePoints.Add(new LinePoint(x + xOffset * halfWidth, testY, startRadius * Math.Max(0.2f, progress), 0.4f));
					index++;
				}
				add = !add;
			}

			treePoints.RemoveRange(topMiddlestXIndex, index - topMiddlestXIndex);

			RadiusLine tree = new RadiusLine(0.15f, treePoints.ToArray());
			//place initial trunk
			tree.Place(_noise, (ushort)ModContent.TileType<LivingBriarWood>());

			//place branches
			bool branchLeft = true;
			float chanceToSwap = 0.5f;
			for(int branchY = (int)(y - height * 0.25); branchY > topY + 8; branchY -= WorldGen.genRand.Next(4, 9)) {
				float progress = 1f - ((y - branchY) / (float)height);
				float mult = GenerateXOffset(branchY);
				float xOffset = mult * halfWidth;
				int branchX = (int)(x + xOffset);
				int stepCount = WorldGen.genRand.Next(halfWidth * 2, halfWidth * 4);
				float velX = branchLeft ? -1f : 1f;
				float chance = WorldGen.genRand.NextFloat(1f);
				if(chance < chanceToSwap) {
					branchLeft = !branchLeft;
					chanceToSwap = 0.5f;
				} else {
					chanceToSwap += 0.1f;
				}
				GenericRunner(branchX, branchY, stepCount, (ushort)ModContent.TileType<BarkTileTile>(), false,
					new RunnerSettings(velX, velX, 0f, 0f, 0.5f, Math.Max(0.6f, 2f * progress)), new RunnerSettings(0f, 0f, -0.1f, 0.1f, 0.8f, 1f),
					(int bx, int by, int step) => BranchLeaves(bx, by, step, stepCount, progress));
			}

			//place roots
			int roots = WorldGen.genRand.Next(12, 20);
			for(int i = 0; i < roots; i++) {
				GenericRunner(x + WorldGen.genRand.Next(-3, 4), y + WorldGen.genRand.Next(-2, 4), WorldGen.genRand.Next(7, 24), (ushort)ModContent.TileType<BarkTileTile>(), true,
					new RunnerSettings(-4f, 4f, 0.02f, 1f, 0.8f, 1f), new RunnerSettings(-0.3f, 0.3f, -0.16f, 0.8f, 0.8f, 1f));
			}
		}

		private void BranchLeaves(int bx, int by, int step, int stepCount, float treeHeightProgress = 1f)
		{
			float stepProg = step / (float)stepCount;
			float radius = (5f - (1f - treeHeightProgress) * 3f) * (0.3f + stepProg * 0.7f);

			int minX = (int)(bx - radius);
			int maxX = (int)(bx + radius);
			int minY = (int)(by - radius);
			int maxY = (int)(by + radius);
			for(int tX = minX; tX <= maxX; tX++) {
				double dX = tX - bx;
				for(int tY = minY; tY <= maxY; tY++) {
					double dY = tY - by;
					Vector2 normalised = Vector2.Normalize(new Vector2((float)dX, (float)dY));
					float radian = (float)Math.Atan2(normalised.Y, normalised.X);
					float bonus = _noise.Noise(radian * 0.05f, 0.15f);
					if(Math.Sqrt(dX * dX + dY * dY) < radius + (bonus * 2f * (1f - stepProg))) {
						Tile tile = Framing.GetTileSafely(tX, tY);
						if(!tile.active()) {
							tile.active(true);
							tile.slope(0);
							tile.type = TileID.LivingMahoganyLeaves;
						}
					}
				}
			}
		}

		private void GenericRunner(float x, float y, int steps, ushort type, bool doNormalise, RunnerSettings initSettings, RunnerSettings stepSettings, Action<int, int, int> perStep = null)
		{
			double strength = (double)WorldGen.genRand.NextFloat(initSettings.strengthModMin, initSettings.strengthModMax);
			Vector2 velocity = new Vector2(WorldGen.genRand.NextFloat(initSettings.velXModMin, initSettings.velXModMax), WorldGen.genRand.NextFloat(initSettings.velYModMin, initSettings.velYModMax));
			for(int i = 0; i < steps; i++) {
				int minX = (int)(x - strength);
				int maxX = (int)(x + strength);
				int minY = (int)(y - strength);
				int maxY = (int)(y + strength);
				for(int tX = minX; tX <= maxX; tX++) {
					double dX = tX - x;
					for(int tY = minY; tY <= maxY; tY++) {
						double dY = tY - y;
						if(Math.Sqrt(dX * dX + dY * dY) < strength) {
							Tile tile = Framing.GetTileSafely(tX, tY);
							tile.active(true);
							tile.slope(0);
							tile.type = type;
						}
					}
				}

				perStep?.Invoke((int)x, (int)y, i);

				strength *= WorldGen.genRand.NextFloat(stepSettings.strengthModMin, stepSettings.strengthModMax);
				if(strength < 0.6) strength = 0.6;
				velocity.X += WorldGen.genRand.NextFloat(stepSettings.velXModMin, stepSettings.velXModMax);
				velocity.Y += WorldGen.genRand.NextFloat(stepSettings.velYModMin, stepSettings.velYModMax);
				if(doNormalise) {
					velocity.Normalize();
				}
				x += velocity.X;
				y += velocity.Y;
			}
		}

		private class RunnerSettings
		{
			public float velXModMin;
			public float velXModMax;
			public float velYModMin;
			public float velYModMax;
			public float strengthModMin;
			public float strengthModMax;
			public RunnerSettings(float vxmmin, float vxmmax, float vymmin, float vymmax, float smmin, float smmax)
			{
				velXModMin = vxmmin;
				velXModMax = vxmmax;
				velYModMin = vymmin;
				velYModMax = vymmax;
				strengthModMin = smmin;
				strengthModMax = smmax;
			}
		}

		//Copied from Terraria source, modified to allow for min and max height trees.
		public static bool GrowTreeCustom(int i, int y, int minHeight, int maxHeight)
		{
			int num;
			int num1 = y;
			while(TileLoader.IsSapling((int)Main.tile[i, num1].type)) {
				num1++;
			}
			if((Main.tile[i - 1, num1 - 1].liquid != 0 || Main.tile[i, num1 - 1].liquid != 0 || Main.tile[i + 1, num1 - 1].liquid != 0) && Main.tile[i, num1].type != 60) {
				return false;
			}
			if(Main.tile[i, num1].nactive() &&
				!Main.tile[i, num1].halfBrick() &&
				Main.tile[i, num1].slope() == 0 &&
				(Main.tile[i, num1].type == 2 || Main.tile[i, num1].type == 23 || Main.tile[i, num1].type == 60 || Main.tile[i, num1].type == 109 || Main.tile[i, num1].type == 147 || Main.tile[i, num1].type == 199 || Main.tile[i, num1].type == 70 || TileLoader.CanGrowModTree((int)Main.tile[i, num1].type)) &&
				(Main.tile[i - 1, num1].active() &&
					(Main.tile[i - 1, num1].type == 2 || Main.tile[i - 1, num1].type == 23 || Main.tile[i - 1, num1].type == 60 || Main.tile[i - 1, num1].type == 109 || Main.tile[i - 1, num1].type == 147 || Main.tile[i - 1, num1].type == 199 || Main.tile[i - 1, num1].type == 70 || TileLoader.CanGrowModTree((int)Main.tile[i - 1, num1].type)) ||
					Main.tile[i + 1, num1].active() &&
					(Main.tile[i + 1, num1].type == 2 || Main.tile[i + 1, num1].type == 23 || Main.tile[i + 1, num1].type == 60 || Main.tile[i + 1, num1].type == 109 || Main.tile[i + 1, num1].type == 147 || Main.tile[i + 1, num1].type == 199 || Main.tile[i + 1, num1].type == 70 || TileLoader.CanGrowModTree((int)Main.tile[i + 1, num1].type))
					)
				) {
				int num2 = 2;
				int num3 = maxHeight;
				if(WorldGen.EmptyTileCheck(i - num2, i + num2, num1 - num3, num1 - 1, 20)) {
					bool flag = false;
					bool flag1 = false;
					int num4 = WorldGen.genRand.Next(minHeight, maxHeight);
					for(int i1 = num1 - num4; i1 < num1; i1++) {
						Main.tile[i, i1].frameNumber((byte)WorldGen.genRand.Next(3));
						Main.tile[i, i1].active(true);
						Main.tile[i, i1].type = 5;
						num = WorldGen.genRand.Next(3);
						int num5 = WorldGen.genRand.Next(10);
						if(i1 == num1 - 1 || i1 == num1 - num4) {
							num5 = 0;
						}
						while(true) {
							if((num5 == 5 ? false : num5 != 7) | !flag) {
								if((num5 == 6 ? false : num5 != 7) | !flag1) {
									break;
								}
							}
							num5 = WorldGen.genRand.Next(10);
						}
						flag = false;
						flag1 = false;
						if(num5 == 5 || num5 == 7) {
							flag = true;
						}
						if(num5 == 6 || num5 == 7) {
							flag1 = true;
						}
						if(num5 == 1) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 0;
								Main.tile[i, i1].frameY = 66;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 0;
								Main.tile[i, i1].frameY = 88;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 0;
								Main.tile[i, i1].frameY = 110;
							}
						} else if(num5 == 2) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 22;
								Main.tile[i, i1].frameY = 0;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 22;
								Main.tile[i, i1].frameY = 22;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 22;
								Main.tile[i, i1].frameY = 44;
							}
						} else if(num5 == 3) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 44;
								Main.tile[i, i1].frameY = 66;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 44;
								Main.tile[i, i1].frameY = 88;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 44;
								Main.tile[i, i1].frameY = 110;
							}
						} else if(num5 == 4) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 22;
								Main.tile[i, i1].frameY = 66;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 22;
								Main.tile[i, i1].frameY = 88;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 22;
								Main.tile[i, i1].frameY = 110;
							}
						} else if(num5 == 5) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 88;
								Main.tile[i, i1].frameY = 0;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 88;
								Main.tile[i, i1].frameY = 22;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 88;
								Main.tile[i, i1].frameY = 44;
							}
						} else if(num5 == 6) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 66;
								Main.tile[i, i1].frameY = 66;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 66;
								Main.tile[i, i1].frameY = 88;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 66;
								Main.tile[i, i1].frameY = 110;
							}
						} else if(num5 != 7) {
							if(num == 0) {
								Main.tile[i, i1].frameX = 0;
								Main.tile[i, i1].frameY = 0;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 0;
								Main.tile[i, i1].frameY = 22;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 0;
								Main.tile[i, i1].frameY = 44;
							}
						} else {
							if(num == 0) {
								Main.tile[i, i1].frameX = 110;
								Main.tile[i, i1].frameY = 66;
							}
							if(num == 1) {
								Main.tile[i, i1].frameX = 110;
								Main.tile[i, i1].frameY = 88;
							}
							if(num == 2) {
								Main.tile[i, i1].frameX = 110;
								Main.tile[i, i1].frameY = 110;
							}
						}
						if(num5 == 5 || num5 == 7) {
							Main.tile[i - 1, i1].active(true);
							Main.tile[i - 1, i1].type = 5;
							num = WorldGen.genRand.Next(3);
							if(WorldGen.genRand.Next(3) >= 2) {
								if(num == 0) {
									Main.tile[i - 1, i1].frameX = 66;
									Main.tile[i - 1, i1].frameY = 0;
								}
								if(num == 1) {
									Main.tile[i - 1, i1].frameX = 66;
									Main.tile[i - 1, i1].frameY = 22;
								}
								if(num == 2) {
									Main.tile[i - 1, i1].frameX = 66;
									Main.tile[i - 1, i1].frameY = 44;
								}
							} else {
								if(num == 0) {
									Main.tile[i - 1, i1].frameX = 44;
									Main.tile[i - 1, i1].frameY = 198;
								}
								if(num == 1) {
									Main.tile[i - 1, i1].frameX = 44;
									Main.tile[i - 1, i1].frameY = 220;
								}
								if(num == 2) {
									Main.tile[i - 1, i1].frameX = 44;
									Main.tile[i - 1, i1].frameY = 242;
								}
							}
						}
						if(num5 == 6 || num5 == 7) {
							Main.tile[i + 1, i1].active(true);
							Main.tile[i + 1, i1].type = 5;
							num = WorldGen.genRand.Next(3);
							if(WorldGen.genRand.Next(3) >= 2) {
								if(num == 0) {
									Main.tile[i + 1, i1].frameX = 88;
									Main.tile[i + 1, i1].frameY = 66;
								}
								if(num == 1) {
									Main.tile[i + 1, i1].frameX = 88;
									Main.tile[i + 1, i1].frameY = 88;
								}
								if(num == 2) {
									Main.tile[i + 1, i1].frameX = 88;
									Main.tile[i + 1, i1].frameY = 110;
								}
							} else {
								if(num == 0) {
									Main.tile[i + 1, i1].frameX = 66;
									Main.tile[i + 1, i1].frameY = 198;
								}
								if(num == 1) {
									Main.tile[i + 1, i1].frameX = 66;
									Main.tile[i + 1, i1].frameY = 220;
								}
								if(num == 2) {
									Main.tile[i + 1, i1].frameX = 66;
									Main.tile[i + 1, i1].frameY = 242;
								}
							}
						}
					}
					int num6 = WorldGen.genRand.Next(3);
					bool flag2 = false;
					bool flag3 = false;
					if(Main.tile[i - 1, num1].nactive() && !Main.tile[i - 1, num1].halfBrick() && Main.tile[i - 1, num1].slope() == 0 && (Main.tile[i - 1, num1].type == 2 || Main.tile[i - 1, num1].type == 23 || Main.tile[i - 1, num1].type == 60 || Main.tile[i - 1, num1].type == 109 || Main.tile[i - 1, num1].type == 147 || Main.tile[i - 1, num1].type == 199 || TileLoader.CanGrowModTree((int)Main.tile[i - 1, num1].type))) {
						flag2 = true;
					}
					if(Main.tile[i + 1, num1].nactive() && !Main.tile[i + 1, num1].halfBrick() && Main.tile[i + 1, num1].slope() == 0 && (Main.tile[i + 1, num1].type == 2 || Main.tile[i + 1, num1].type == 23 || Main.tile[i + 1, num1].type == 60 || Main.tile[i + 1, num1].type == 109 || Main.tile[i + 1, num1].type == 147 || Main.tile[i + 1, num1].type == 199 || TileLoader.CanGrowModTree((int)Main.tile[i + 1, num1].type))) {
						flag3 = true;
					}
					if(!flag2) {
						if(num6 == 0) {
							num6 = 2;
						}
						if(num6 == 1) {
							num6 = 3;
						}
					}
					if(!flag3) {
						if(num6 == 0) {
							num6 = 1;
						}
						if(num6 == 2) {
							num6 = 3;
						}
					}
					if(flag2 && !flag3) {
						num6 = 2;
					}
					if(flag3 && !flag2) {
						num6 = 1;
					}
					if(num6 == 0 || num6 == 1) {
						Main.tile[i + 1, num1 - 1].active(true);
						Main.tile[i + 1, num1 - 1].type = 5;
						num = WorldGen.genRand.Next(3);
						if(num == 0) {
							Main.tile[i + 1, num1 - 1].frameX = 22;
							Main.tile[i + 1, num1 - 1].frameY = 132;
						}
						if(num == 1) {
							Main.tile[i + 1, num1 - 1].frameX = 22;
							Main.tile[i + 1, num1 - 1].frameY = 154;
						}
						if(num == 2) {
							Main.tile[i + 1, num1 - 1].frameX = 22;
							Main.tile[i + 1, num1 - 1].frameY = 176;
						}
					}
					if(num6 == 0 || num6 == 2) {
						Main.tile[i - 1, num1 - 1].active(true);
						Main.tile[i - 1, num1 - 1].type = 5;
						num = WorldGen.genRand.Next(3);
						if(num == 0) {
							Main.tile[i - 1, num1 - 1].frameX = 44;
							Main.tile[i - 1, num1 - 1].frameY = 132;
						}
						if(num == 1) {
							Main.tile[i - 1, num1 - 1].frameX = 44;
							Main.tile[i - 1, num1 - 1].frameY = 154;
						}
						if(num == 2) {
							Main.tile[i - 1, num1 - 1].frameX = 44;
							Main.tile[i - 1, num1 - 1].frameY = 176;
						}
					}
					num = WorldGen.genRand.Next(3);
					if(num6 == 0) {
						if(num == 0) {
							Main.tile[i, num1 - 1].frameX = 88;
							Main.tile[i, num1 - 1].frameY = 132;
						}
						if(num == 1) {
							Main.tile[i, num1 - 1].frameX = 88;
							Main.tile[i, num1 - 1].frameY = 154;
						}
						if(num == 2) {
							Main.tile[i, num1 - 1].frameX = 88;
							Main.tile[i, num1 - 1].frameY = 176;
						}
					} else if(num6 == 1) {
						if(num == 0) {
							Main.tile[i, num1 - 1].frameX = 0;
							Main.tile[i, num1 - 1].frameY = 132;
						}
						if(num == 1) {
							Main.tile[i, num1 - 1].frameX = 0;
							Main.tile[i, num1 - 1].frameY = 154;
						}
						if(num == 2) {
							Main.tile[i, num1 - 1].frameX = 0;
							Main.tile[i, num1 - 1].frameY = 176;
						}
					} else if(num6 == 2) {
						if(num == 0) {
							Main.tile[i, num1 - 1].frameX = 66;
							Main.tile[i, num1 - 1].frameY = 132;
						}
						if(num == 1) {
							Main.tile[i, num1 - 1].frameX = 66;
							Main.tile[i, num1 - 1].frameY = 154;
						}
						if(num == 2) {
							Main.tile[i, num1 - 1].frameX = 66;
							Main.tile[i, num1 - 1].frameY = 176;
						}
					}
					if(WorldGen.genRand.Next(8) == 0) {
						num = WorldGen.genRand.Next(3);
						if(num == 0) {
							Main.tile[i, num1 - num4].frameX = 0;
							Main.tile[i, num1 - num4].frameY = 198;
						}
						if(num == 1) {
							Main.tile[i, num1 - num4].frameX = 0;
							Main.tile[i, num1 - num4].frameY = 220;
						}
						if(num == 2) {
							Main.tile[i, num1 - num4].frameX = 0;
							Main.tile[i, num1 - num4].frameY = 242;
						}
					} else {
						num = WorldGen.genRand.Next(3);
						if(num == 0) {
							Main.tile[i, num1 - num4].frameX = 22;
							Main.tile[i, num1 - num4].frameY = 198;
						}
						if(num == 1) {
							Main.tile[i, num1 - num4].frameX = 22;
							Main.tile[i, num1 - num4].frameY = 220;
						}
						if(num == 2) {
							Main.tile[i, num1 - num4].frameX = 22;
							Main.tile[i, num1 - num4].frameY = 242;
						}
					}
					WorldGen.RangeFrame(i - 2, num1 - num4 - 1, i + 2, num1 + 1);
					if(Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, i, (int)((double)num1 - (double)num4 * 0.5), num4 + 1, TileChangeType.None);
					}
					return true;
				}
			}
			return false;
		}
	}
}