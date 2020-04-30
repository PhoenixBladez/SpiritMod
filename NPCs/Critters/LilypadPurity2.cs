using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Critters
{
	public class LilypadPurity2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lilypad");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 20;
			npc.damage = 0;
			npc.defense = 1000;
			npc.lifeMax = 1;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
			npc.noGravity = false;
			npc.alpha = 40;
            npc.behindTiles = true;
            npc.dontCountMe = true;
			npc.dontTakeDamage = true;
		}
        public float num42;
        bool collision = false;
		public override void AI()
		{
            npc.spriteDirection = -npc.direction;
            int npcXTile = (int)(npc.Center.X / 16);
            int npcYTile = (int)(npc.Center.Y / 16);
            for (int y = npcYTile; y > Math.Max(0, npcYTile - 100); y--)
            {
                if (Main.tile[npcXTile, y].liquid != 255)
                {
                    int liquid = (int)Main.tile[npcXTile, y].liquid;
                    float up = (liquid / 255f) * 16f;
                    npc.position.Y = (y + 1) * 16f - up;
                    break;
                }
            }
            if (!collision)
            {
                npc.velocity.X = .5f * Main.windSpeed;
            }
            else
            {
                npc.velocity.X = -.5f * Main.windSpeed;
            }
            if (npc.collideX || npc.collideY)
            {
                npc.velocity.X *= -1f;
                if (!collision)
                {
                    collision = true;
                }
                else
                {
                    collision = false;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY - 10), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
            {
                return !player.GetSpiritPlayer().ZoneReach && player.ZoneOverworldHeight && spawnInfo.water && Main.dayTime && !player.ZoneJungle && !player.ZoneBeach && !player.ZoneCrimson && !player.ZoneCorrupt ? 1.3f : 0f;
            }
            return 0f;
        }
    }
}
