﻿using System.Collections;

public class Word
{
	public int WordId { get; set; }

	public string Name { get; set; }

	public string Resource { get; set; }

	public ArrayList Syllables = new ArrayList();

	public int SyllablesNumber { get; set; }

	public float MaxReadDif { get; set; }

	public float LearningDegreeRead { get; set; }

	public short LearnedRead { get; set; }

	public float MaxWriteDif { get; set; }

	public float LearningDegreeWrite { get; set; }

	public short LearnedWrite { get; set; }

	public Word()
	{
	}
}