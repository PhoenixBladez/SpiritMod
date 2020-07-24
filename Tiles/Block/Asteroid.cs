using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Tiles.Block
{
	public class Asteroid : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(99, 79, 49));
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			minPick = 100;
			drop = ModContent.ItemType<AsteroidBlock>();
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			Player player = Main.LocalPlayer;
			if (player.inventory[player.selectedItem].type == ItemID.ReaverShark) {
				return false;
			}
			return true;
		}
		//UNCOMMENT THIS IF YOU WANT CRYSTALS TO GROW ON REGULAR ASTEROIDS
		/*    public override void RandomUpdate(int i, int j)
            {
                if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(50) == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            ReachGrassTile.PlaceObject(i, j - 1, mod.TileType("GlowShard1"));
                            NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("GlowShard1"), 0, 0, -1, -1);
                            break;
                        case 1:
                            ReachGrassTile.PlaceObject(i, j - 1, ModContent.TileType<GreenShard>());
                            NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<GreenShard>(), 0, 0, -1, -1);
                            break;
                        case 2:
                            ReachGrassTile.PlaceObject(i, j - 1, ModContent.TileType<PurpleShard>());
                            NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<PurpleShard>(), 0, 0, -1, -1);
                            break;
                    }
                }
            }*/
	}
}