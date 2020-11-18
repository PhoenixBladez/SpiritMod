using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Material;
using Terraria;
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
			npc.width = 64; //324
			npc.height = 56; //216
			bossBag = ModContent.ItemType<SteamRaiderBag>();
			npc.boss = true;
			npc.damage = 0;
			npc.defense = 12;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/null");
			npc.noTileCollide = true;
			npc.dontTakeDamage = true;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit4;
			npc.value = 160f;
			npc.knockBackResist = .16f;
			npc.noGravity = true;
			npc.dontCountMe = true;
		}
		int timeLeft = 200;
		float alphaCounter;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
			float sineAdd = alphaCounter + 2;
			Vector2 drawPos1 = npc.Center - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
			Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .65f), SpriteEffects.None, 0f);
			return true;
		}
		public override void AI()
		{
			alphaCounter += 0.025f;
			npc.alpha = 255 - timeLeft;
			if (timeLeft == 200) {
				npc.rotation = 3.14f;
			}
			npc.rotation += Main.rand.Next(-20, 20) / 100f;
			Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)));
			if (timeLeft < 50) {
				Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)));
			}
			timeLeft--;
			if (timeLeft <= 0) {

				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.Heart);
				npc.DropItem(ItemID.LesserHealingPotion, 10, 12);
				if (!Main.expertMode) {
					npc.DropItem(ModContent.ItemType<CosmiliteShard>(), 6, 10);
					npc.DropItem(ModContent.ItemType<StarplateMask>(), 1f / 7);
					npc.DropItem(ModContent.ItemType<Trophy3>(), 1f / 10);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Starplate/Starplate1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Starplate/Starplate2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Starplate/Starplate3"), 1f);
				Main.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)npc.position.X, (int)npc.position.Y);
				//  Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
				for (int i = 0; i < 90; i++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 226, Main.rand.Next(-25, 25), Main.rand.Next(-13, 13));
				}
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 4f;
				direction.Y *= -4f;

				Main.NewText("Starplate Voyager has been defeated!", 175, 75, 255, false);
				npc.life = 0;
			}
		}
		public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
        }
	}
}