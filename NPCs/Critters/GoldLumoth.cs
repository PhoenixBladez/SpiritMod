using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Critters
{
	public class GoldLumoth : ModNPC
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Lumoth");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.dontCountMe = true;
            Main.npcCatchable[NPC.type] = true;
            NPC.catchItem = (short)Mod.Find<ModItem>("GoldLumothItem").Type;
            NPC.knockBackResist = .45f;
            NPC.aiStyle = 64;
            NPC.npcSlots = 0;
            NPC.noGravity = true;
            AIType = NPCID.Firefly;
            Main.npcFrameCount[NPC.type] = 4;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
                             drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, ModContent.Request<Texture2D>("SpiritMod/NPCs/Critters/Lumoth_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 32;
                NPC.height = 32;
                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
                for (int num621 = 0; num621 < 20; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.GoldCoin, 0f, 0f, 100, default, 1f);
                    Main.dust[num622].velocity *= .1f;
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.dust[num622].scale = 0.9f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.000763f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldCoin);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), .4f, .4f, .4f);
        }
        public override void OnKill()
        {
            if (Main.rand.Next(3) == 1)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Brightbulb").Type, 1);
            }
        }
    }
}
