﻿namespace ProvaPub.Services
{
	public class RandomService
	{
		int seed;
		public RandomService()
		{
			seed = Guid.NewGuid().GetHashCode();
		}
		public int GetRandom()
		{
			//retirado o comando seed
			return new Random().Next(100);
		}

	}
}
