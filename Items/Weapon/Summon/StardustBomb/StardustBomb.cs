using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Weapon.Summon.StardustBomb
{
	public class StardustBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova");
			Tooltip.SetDefault("Summons a collapsing star\nThe bomb can be targeted\nDestroy the bomb to release a powerful explosion");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff);
			item.damage = 3500;
			item.mana = 12;
			item.width = 40;
			item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 8, 0, 0);
			item.rare = 10;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.summon = true;
			item.shootSpeed = 1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentStardust, 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc.ai[0] == player.whoAmI && npc.type == ModContent.NPCType<StardustBombNPC>())
				{
					npc.active = false;
					npc.netUpdate = true;
				}
			}
			NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y + 100, ModContent.NPCType<StardustBombNPC>(), 0, player.whoAmI);
			return false;
		}
	}

	internal class StardustBombNPC : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova");
			Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.width = 158;
            npc.height = 197;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 3000;
            npc.damage = 0;
            npc.defense = 0;
			npc.HitSound = SoundID.NPCHit3;
            npc.noTileCollide = false;
			npc.noGravity = true;
            npc.dontCountMe = true;
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		float shakeRotation;
		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Cyan.R * 0.005f, Color.Cyan.G * 0.005f, Color.Cyan.B * 0.005f);
			float shakeMult = (float)Math.Sqrt(1 - (npc.life / 4000f)) * 0.05f;
			shakeRotation += Main.rand.NextFloat(0 - shakeMult, shakeMult);
			npc.ai[1]++;
			npc.ai[2]++;
			if (npc.ai[2] > npc.life / 100)
			{
				npc.ai[2] = 0;
				float size = 2 - (npc.life / 2000f);
				DustHelper.DrawStar(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), 206, 5, size, size, size);
				npc.netUpdate = true;
			}
			if (npc.ai[1] == 20)
				Main.PlaySound(SoundID.DD2_EtherianPortalIdleLoop, npc.Center);
			npc.rotation = npc.ai[1] / 60f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Player player = Main.player[(int)npc.ai[0]];
			if (npc.life <= 0)
			{
				Main.PlaySound(SoundID.Item92, npc.Center);
				for (int i = 0; i < 2; i++)
				{
					for (int j = 1; j < 5; ++j)
					{
						float randFloat = Main.rand.NextFloat(6.28f);
						Gore.NewGore(npc.Center + (randFloat.ToRotationVector2() * 60), randFloat.ToRotationVector2() * 16, mod.GetGoreSlot("Gores/StarbombGore/StarbombGore" + j), 1f);
					}
				}
				Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, mod.ProjectileType("StarShockwave"), (int)(3500 * player.minionDamage), 0, player.whoAmI);
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Thunder"), npc.Center);
				SpiritMod.tremorTime = 15;
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (!projectile.minion)
				damage = 0;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (!item.summon)
				damage = 0; //dk why you could melee hit with a summon item but hey it's fine
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			 Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                float breakCounter = 1 - (npc.life / 4000f);
				SpiritMod.CircleNoise.Parameters["breakCounter"].SetValue(breakCounter);
				SpiritMod.CircleNoise.Parameters["rotation"].SetValue(0 - (0 / 200f) + shakeRotation);
				SpiritMod.CircleNoise.Parameters["colorMod"].SetValue(Color.Silver.ToVector4());
				SpiritMod.CircleNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
				SpiritMod.CircleNoise.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0, npc.gfxOffY), null, Color.White, 0f, new Vector2(50, 50), 0.75f + (breakCounter / 2), SpriteEffects.None, 0f);

				Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			Color bloomColor = Color.Cyan;
			bloomColor.A = 0;
			Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0, npc.gfxOffY), null, bloomColor, 0f, new Vector2(50, 50), 0.45f + (breakCounter / 4), SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(
                mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Star"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				new Rectangle(0,0,48,52),
				Color.White,
				0 - (0 / 200f) + shakeRotation,
				new Vector2(28,26),
				npc.scale,
				SpriteEffects.None, 0
			);
			Main.spriteBatch.Draw(
                mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Star"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				new Rectangle(0,52,48,52),
				Color.White * breakCounter,
				0 - (0 / 200f) + shakeRotation,
				new Vector2(28,26),
				npc.scale,
				SpriteEffects.None, 0
			);

            Main.spriteBatch.Draw(
                mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame,
				drawColor,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale,
				SpriteEffects.None, 0
			);
            return false;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float num107 = 0f;

            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
			Color color28 = color29;
            color28 = npc.GetAlpha(color28);
            color28 *= 0.5f;

            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = npc.Center + ((float)num103 / 4f * 6.28318548f + npc.rotation).ToRotationVector2() * (2f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY);
                Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }

            num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;

            spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
            color28 = color29;
            color28 = npc.GetAlpha(color28);
            color28 *= 1f - num107;

            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = npc.Center + ((float)num103 / 4f * 6.28318548f + npc.rotation).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY);
                Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }
        }
	}
}
