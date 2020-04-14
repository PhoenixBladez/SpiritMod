using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;
using SpiritMod;

namespace SpiritMod.Tiles.Block
{
    public class SpaceJunkTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            soundType = 21;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(87, 85, 81));
            drop = mod.ItemType("SpaceJunkItem");
            dustType = 54;
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = 54;
            makeDust = true;
        }
        /*public override void FloorVisuals(Player player)
        {
            player.AddBuff(BuffID.Bleeding, 300);
            player.life -= 1;
        }*/
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                if (Main.rand.Next(20) == 0)
                {
                    int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 10), Main.rand.Next(-2,2), Main.rand.Next(-2, 2), 54, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division

                    Main.dust[d].velocity *= .8f;
                    Main.dust[d].noGravity = true;
                }
            }
        }
    }
}

