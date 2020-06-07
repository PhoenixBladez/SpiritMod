using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;
using SpiritMod;
using System.Linq;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Buffs;

namespace SpiritMod.Tiles.Block
{
    public class CreepingIceTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(163, 224, 240));
            drop = ModContent.ItemType<CreepingIce>();
            dustType = 51;
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = 51;
            makeDust = true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.LocalPlayer;
            int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
            if (distance < 54)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
                
            }
        }
        public override void FloorVisuals(Player player)
        {
            player.AddBuff(BuffID.Chilled, 1200);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                float distance = 15 * 16;
                List<NPC> foundNPCs = Main.npc.Where(n => n.active && n.DistanceSQ(new Vector2(i * 16, j * 16)) < distance * distance).OrderBy(n => n.DistanceSQ(new Vector2(i * 16, j * 16)) < distance * distance).ToList();
                foreach (var foundNPC in foundNPCs)
                {
                    if (foundNPC != null)
                    {
                        int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), foundNPC.Center);
                        if (distance1 < 22)
                        {
                            foundNPC.AddBuff(ModContent.BuffType<MageFreeze>(), 20);
                        }
                    }
                }
             
            }
        }
    }
}

