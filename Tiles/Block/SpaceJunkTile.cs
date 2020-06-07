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
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(87, 85, 81));
            drop = ModContent.ItemType<SpaceJunkItem>();
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
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.LocalPlayer;
            int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
            if (distance < 54)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
                int debuffsWeak = Main.rand.Next(new int[] { 30, 33 });
                int debuffStrong = Main.rand.Next(new int[] { 144, 80, 36, 196, 164 });
                int buffs = Main.rand.Next(new int[] { 5, 8, 18, 114, 11 });
                if (Main.rand.Next(6) == 0)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        player.AddBuff(debuffStrong, 70);
                    }
                    else
                    {
                        player.AddBuff(debuffsWeak, 300);
                    }
                }
                if (Main.rand.Next(3) == 0)
                {
                    player.AddBuff(buffs, 540);
                }
            }
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

