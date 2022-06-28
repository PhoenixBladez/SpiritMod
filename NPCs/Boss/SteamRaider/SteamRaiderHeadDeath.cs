using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.StarplateDrops;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class SteamRaiderHeadDeath : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Voyager");
		}

		public override void SetDefaults()
		{
			NPC.width = 64; //324
			NPC.height = 56; //216
			bossBag = ModContent.ItemType<SteamRaiderBag>();
			NPC.boss = true;
			NPC.damage = 0;
			NPC.defense = 12;
			Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/null");
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			NPC.lifeMax = 65;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.value = 160f;
			NPC.knockBackResist = .16f;
			NPC.noGravity = true;
			NPC.dontCountMe = true;
		}
		int timeLeft = 200;
		float alphaCounter;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
			float sineAdd = alphaCounter + 2;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[49].Value, (NPC.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .65f), SpriteEffects.None, 0f);
			return true;
		}
        int playersActive;
		public override void AI()
		{
			alphaCounter += 0.025f;
			NPC.alpha = 255 - timeLeft;
			if (timeLeft == 200) {
				NPC.rotation = 3.14f;
			}
			NPC.rotation += Main.rand.Next(-20, 20) / 100f;
			Dust.NewDustPerfect(NPC.Center, 226, new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)));
			if (timeLeft < 50) {
				Dust.NewDustPerfect(NPC.Center, 226, new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)));
			}
			timeLeft--;
			if (timeLeft <= 0) {
                if (Main.expertMode) {
					if (Main.netMode != NetmodeID.SinglePlayer) {
						for (int i = 0; i < Main.maxPlayers; i++) {
							if (Main.player[i].active)
                                playersActive++;  
                        }
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<SteamRaiderBag>(), playersActive);
                    }
					else { 
                    NPC.DropBossBags();
					}
                }
                NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.Heart);
				NPC.DropItem(ItemID.LesserHealingPotion, 10, 12);
				if (!Main.expertMode) {
					NPC.DropItem(ModContent.ItemType<CosmiliteShard>(), 6, 10);
					NPC.DropItem(ModContent.ItemType<StarplateMask>(), 1f / 7);
					NPC.DropItem(ModContent.ItemType<Trophy3>(), 1f / 10);
				}
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Starplate/Starplate1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Starplate/Starplate2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Starplate/Starplate3").Type, 1f);
				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)NPC.position.X, (int)NPC.position.Y);
				//  Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
				for (int i = 0; i < 90; i++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, Main.rand.Next(-25, 25), Main.rand.Next(-13, 13));
				}
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X *= 4f;
				direction.Y *= -4f;

				NPC.life = 0;
                NPC.active = false;
			}
		}
		public override void OnKill()
        {
        }
	}
}