using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.StarplateDrops
{
	public class Glowstone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			AddMapEntry(new Color(156, 102, 36));
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cosmilite");
			ItemDrop = ModContent.ItemType<CosmiliteShard>();
			HitSound = SoundID.Tink;
			DustType = DustID.Electric;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Player player = Main.LocalPlayer;
			int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
			if (distance < 54) {
				SoundEngine.PlaySound(SoundID.NPCHit4);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.039f * 2;
			g = .1f * 2;
			b = .1275f * 2;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/BossLoot/StarplateDrops/Glowstone_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if (!MyWorld.downedRaider) {
				return false;
			}
			return true;
		}
        public override bool CanExplode(int i, int j)
        {
            if (!MyWorld.downedRaider)
            {
                return false;
            }
            return true;
        }
    }
}