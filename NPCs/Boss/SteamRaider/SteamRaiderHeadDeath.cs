using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
		float timeLeft = 200;
		public override void AI()
		{
			if (timeLeft == 200) {
				npc.rotation = 3.14f;
			}
			npc.rotation += Main.rand.Next(-20, 20) / 100f;
			timeLeft-= 0.7f;
			if (timeLeft <= 0) {
				Player player = Main.player[Main.myPlayer];
				player.GetModPlayer<MyPlayer>().Shake += 16;
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			float breakCounter = 1 - (timeLeft / 200f);
			Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SpiritMod.CircleNoise.Parameters["breakCounter"].SetValue(breakCounter);
            SpiritMod.CircleNoise.Parameters["colorMod"].SetValue(new Vector4(0.5f, 0.8f, 1f, 1f));
            SpiritMod.CircleNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
            SpiritMod.CircleNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, Color.White, 0f, new Vector2(50, 50), 4 + (breakCounter / 2), SpriteEffects.None, 0f);
			 Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Texture2D whiteTexture = SpiritMod.instance.GetTexture("NPCs/Boss/SteamRaider/WhiteHead");
			spriteBatch.Draw(whiteTexture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White * ((breakCounter * 2) - 1), npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
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