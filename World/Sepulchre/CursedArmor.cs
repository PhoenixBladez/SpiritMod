using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Enchanted_Armor;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.World.Sepulchre
{
	public class CursedArmor : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;

			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(0, 3);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();

			MineResist = 0.2f;

			name.SetDefault("Cursed Armor");
			AddMapEntry(Color.DarkSlateGray, name);
		}
		public override bool IsTileDangerous(int i, int j, Player player) => true;
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new LegacySoundStyle(SoundID.NPCDeath, 6).WithPitchVariance(0.2f), new Vector2(i * 16, j * 16));
			NPC npc = Main.npc[NPC.NewNPC(new EntitySource_TileBreak(i, j)(i + 1) * 16, (j + 4) * 16, ModContent.NPCType<Enchanted_Armor>())];
			npc.velocity = Vector2.Zero;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
		}
	}
}
