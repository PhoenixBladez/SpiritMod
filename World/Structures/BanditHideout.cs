using SpiritMod.NPCs.Town;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.World
{
	public class BanditHideout : AStructure
	{
		public override int OffsetX => -3;
		public override int OffsetY => -28;
		public override int[,] Tiles => new int[,] {
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,2,2,2,2,2,2,2,2,5,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,0,0,0,0,0,0,0,2,2,5,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,3,0,0,0,0,0,0,0,0,3,2,2,5,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,3,0,0,0,0,0,0,0,0,3,0,2,2,5,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,0,3,0,0,0,0,0,0,0,0,3,0,0,2,2,5,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,0,0,3,0,0,0,0,0,0,0,0,3,0,0,0,2,2,5,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,1,1,1,1,1,5,5,5,5,5,5,5,5,1,1,1,1,1,2,2,5},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,4,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,4,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,6,6,3,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,6,6,4,9},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,6,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,6,4,9},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,0,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,4,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,4,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,4,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,9,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,9,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,9,9},
			{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,9,9},
			{0,0,0,6,6,6,6,6,6,6,6,6,9,9,9,9,9,9,9,9,9,6,3,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,6,9,9,9},
			{0,0,0,2,1,1,1,1,1,1,1,2,7,7,7,7,7,7,7,7,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
		};
		public override int[,] Slopes => new int[,] {
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		};
		public override int[,] Walls => new int[,] {
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,2,2,2,2,2,2,2,2,5,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,2,2,5,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,2,2,5,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,4,4,2,2,5,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,5,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,5,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,1,1,1,1,1,4,4,4,4,4,4,4,4,1,1,1,1,1,2,2,5},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,6,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
		};
		public override int[,] Furniture => new int[,] {
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,7,0,0,0,0,7,0,0,2,2,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,17,0,5,0,0,5,0,5,0,5,0,5,0,0,2,2,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,10,0,0,0,1,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,8,12,0,0,4,0,0,9,0,0,3,0,0,6,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,11,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,16,0,0,15,0,0,11,0,0,0,14,0,0,0,0,13,0,0},
			{0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		};

		public override bool Generate()
		{
			bool placed = false;
			int attempts = 0;
			while (!placed && attempts++ < 100000) {
				// Select a place in the first 4th of the world, avoiding the oceans
				int towerX = WorldGen.genRand.Next(300, Main.maxTilesX / 4); // from 50 since there's a unaccessible area at the world's borders
																			 // 50% of choosing the last 4th of the world
																			 // Choose which side of the world to be on randomly
				if (WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}

				//Start at 200 tiles above the surface instead of 0, to exclude floating islands
				int towerY = (int)Main.worldSurface - 200;

				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}

				// If we went under the world's surface, try again
				if (towerY > Main.worldSurface) {
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the hideout on doesn't match what we want, try again
				if (!(tile.TileType == TileID.Dirt
					|| tile.TileType == TileID.Grass
					|| tile.TileType == TileID.Stone
					|| tile.TileType == TileID.Mud
					|| tile.TileType == TileID.CrimsonGrass
					|| tile.TileType == TileID.CorruptGrass
					|| tile.TileType == TileID.JungleGrass
					|| tile.TileType == TileID.Sand
					|| tile.TileType == TileID.Crimsand
					|| tile.TileType == TileID.Ebonsand
					|| tile.TileType == TileID.SnowBlock)) {
					continue;
				}

				// Don't place the hideout if the area isn't flat
				if (!MyWorld.CheckFlat(towerX, towerY, Tiles.GetLength(1), 3))
					continue;

				Place(towerX, towerY);
				int num = NPC.NewNPC((towerX + 31) * 16, (towerY - 20) * 16, ModContent.NPCType<BoundRogue>());
				Main.npc[num].homeTileX = -1;
				Main.npc[num].homeTileY = -1;
				Main.npc[num].direction = 1;
				Main.npc[num].homeless = true;

				placed = true;
			}
			if (!placed) SpiritMod.Instance.Logger.Error("Worldgen: FAILED to place Bandit Hideout, ground not flat enough?");
			return placed;
		}

		protected override TileData TileMap(int tile, int x, int y)
		{
			switch (tile) {
				case 1:
					return new TileData(TileID.WoodBlock);
				case 2:
					return new TileData(TileID.GrayBrick);
				case 3:
					return new TileData(TileID.WoodenBeam);
				case 4:
					return new TileData(TileID.Rope);
				case 5:
					return new TileData(TileID.Platforms, 12);
				case 7:
					return new TileData(TileID.Platforms, 0, false);
				case 6:
				case 9:
					return new TileData(-1);
			}
			return null;
		}

		protected override TileData FurnitureMap(int tile, int x, int y)
		{
			switch (tile) {
				case 4:
					return new ObjectData(TileID.Furnaces);
				case 5:
					return new ObjectData(TileID.Pots);
				case 6:
					return new TileData(TileID.ClosedDoor, 13);
				case 7:
					return new ObjectData(TileID.Painting3X3, Main.rand.Next(44, 45));
				case 8:
					return new ObjectData(TileID.Kegs);
				case 9:
					// TODO: Add this chest tile so this is valid
					//WorldGen.PlaceChest(k, l, (ushort)TileType<BanditChest>(), false, 0); // Gold Chest
					break;
				case 10:
					return new ObjectData(TileID.HangingLanterns, 6);
				case 11:
					return new ObjectData(TileID.Campfire);
				case 12:
					return new ObjectData(TileID.Chairs);
				case 13:
					return new ObjectData(TileID.LargePiles2, 28);
				case 14:
					return new ObjectData(TileID.LargePiles2, 26);
				case 15:
					return new ObjectData(TileID.LargePiles2, 27);
				case 16:
					return new ObjectData(TileID.LargePiles2, 23);
				case 17:
					return new ObjectData(TileID.FishingCrate);
			}
			return null;
		}

		protected override WallData WallMap(int wall, int x, int y)
		{
			switch (wall) {
				case 4:
					return new WallData(WallID.Wood);
				case 6:
					return new WallData(WallID.WoodenFence);
			}
			return new WallData(-1);
		}
	}
}