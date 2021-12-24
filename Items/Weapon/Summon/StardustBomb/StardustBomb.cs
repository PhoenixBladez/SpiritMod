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
			Tooltip.SetDefault("Summons a collapsing star\nThe star can be targeted\ndeal summon damage to the star to release a powerful explosion");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff);
			item.damage = 0;
			item.mana = 12;
			item.width = 40;
			item.height = 40;
			item.value = Item.sellPrice(0, 8, 0, 0);
			item.rare = ItemRarityID.Red;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item20;
			item.summon = true;
			item.shootSpeed = 10f;
			item.noUseGraphic = true;
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
			int npcindex = NPC.NewNPC((int)position.X, (int)position.Y + 100, ModContent.NPCType<StardustBombNPC>(), 0, player.whoAmI);
			NPC npc2 = Main.npc[npcindex];
			npc2.velocity = new Vector2(speedX, speedY);
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
		int returnCounter;

		int boomdamage;

		float shrinkCounter = 0.25f;

		bool shrinking;
        public override void SetDefaults()
        {
            npc.width = 158;
            npc.height = 197;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 30000;
            npc.damage = 0;
            npc.defense = 0;
			npc.HitSound = SoundID.NPCHit3;
            npc.noTileCollide = true;
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
		public override void AI()
		{
			Player player = Main.player[(int)npc.ai[0]];
			returnCounter++;
			if (returnCounter == 200)
			{
				if (Explode())
					npc.active = false;
				else
					shrinking = true;
			}
			npc.velocity *= 0.97f;
			npc.rotation += 0.03f;
			Lighting.AddLight(npc.Center, Color.Cyan.R * 0.005f, Color.Cyan.G * 0.005f, Color.Cyan.B * 0.005f);
			npc.ai[1]++;
			if (npc.ai[1] == 20)
				Main.PlaySound(SoundID.DD2_EtherianPortalIdleLoop, npc.Center);
			if (shrinking)
			{
				shrinkCounter += 0.1f;
				npc.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
				if (npc.scale < 0.3f)
				{
					npc.active = false;
				}
			}
			else
				npc.scale = MathHelper.Min(npc.ai[1] / 15f, 1);
		}

		private bool Explode()
		{
			if (boomdamage == 0)
				return false;
			Player player = Main.player[(int)npc.ai[0]];
			Main.PlaySound(SoundID.Item92, npc.Center);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 1; j < 5; ++j)
				{
					float randFloat = Main.rand.NextFloat(6.28f);
					Gore.NewGore(npc.Center + (randFloat.ToRotationVector2() * 60), randFloat.ToRotationVector2() * 16, mod.GetGoreSlot("Gores/StarbombGore/StarbombGore" + j), 1f);
				}
			}
			Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, mod.ProjectileType("StarShockwave"), (int)(boomdamage * player.minionDamage * 0.5f), 0, player.whoAmI);
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Thunder"), npc.Center);
			SpiritMod.tremorTime = 15;
			return true;
		}

		public override bool? CanBeHitByItem(Player player, Item item) => false;

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.minion)
				return base.CanBeHitByProjectile(projectile);
			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			boomdamage += (int)damage;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Vector2 scale = new Vector2(1,1);

			Color bloomColor = Color.Cyan;
			bloomColor.A = 0;
			Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0, npc.gfxOffY), null, bloomColor, npc.rotation, new Vector2(50, 50), 0.45f * scale * npc.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(
                mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Star"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				new Rectangle(0,0,48,52),
				Color.White,
				npc.rotation,
				new Vector2(28,26),
				npc.scale * scale,
				SpriteEffects.None, 0
			);

            Main.spriteBatch.Draw(
                mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame,
				drawColor,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale * scale,
				SpriteEffects.None, 0
			);
			return false;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Vector2 scale = new Vector2(1, 1);

			float num107 = 0f;

            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
			Color color28 = color29;
            color28 = npc.GetAlpha(color28);
            color28 *= 0.5f;

            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = npc.Center + ((float)num103 / 4f * 6.28318548f + npc.rotation).ToRotationVector2() * (2f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY);
                Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale * scale, spriteEffects3, 0f);
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
                Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Summon/StardustBomb/StardustBombNPC_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale * scale, spriteEffects3, 0f);
            }
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
	}
}
