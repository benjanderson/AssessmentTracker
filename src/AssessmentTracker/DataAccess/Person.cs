﻿namespace AssessmentTracker.DataAccess
{
	public class Person : IEntity
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}