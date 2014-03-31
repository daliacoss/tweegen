#include "Passages.h"

Passage p_StoryTitle = {
	1, {
		{
			4, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"Arthur's Dream of the Dragon and the B", "ear", "", "", }
		},

	}
};
Passage p_Start = {
	3, {
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_completely_equipped_cabin,
			{"Arthur", }
		},
		{
			5, false, {true,false,false,false,false,false,false,false,false,false,}, &p_completely_equipped_cabin,
			{", on a huge vessel", "with a host of knig", "hts", "", "", }
		},

	}
};
Passage p_completely_equipped_cabin = {
	2, {
		{
			4, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King Arthur, on a huge vessel", "with a h", "ost of knights,", "Was enclosed in a ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_richly,
			{"completely equipped cabin", }
		},

	}
};
Passage p_richly = {
	3, {
		{
			6, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King Arthur, on a huge vessel", "with a h", "ost of knights,", "Was enclosed in a comp", "letely equipped cabin,", "Resting on a ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_sent_him_to_sleep,
			{"richly", }
		},
		{
			3, false, {true,false,false,false,false,false,false,false,false,false,}, &p_sent_him_to_sleep,
			{" arrayed bed", "", "", }
		},

	}
};
Passage p_sent_him_to_sleep = {
	3, {
		{
			10, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King Arthur, on a huge vessel", "with a h", "ost of knights,", "Was enclosed in a comp", "letely equipped cabin,", "Resting on a ri", "chly arrayed bed.", "And the swaying on t", "he sea", "", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_dreamed,
			{"sent him to sleep", }
		},
		{
			3, false, {true,false,false,false,false,false,false,false,false,false,}, &p_dreamed,
			{".", "", "", }
		},

	}
};
Passage p_dreamed = {
	2, {
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_dragon,
			{"dreamed", }
		},

	}
};
Passage p_dragon = {
	2, {
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_dreadful,
			{"dragon", }
		},

	}
};
Passage p_dreadful = {
	3, {
		{
			2, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_driving_over_the_deep,
			{"dreadful", }
		},
		{
			3, false, {true,false,false,false,false,false,false,false,false,false,}, &p_driving_over_the_deep,
			{" to behold", "", "", }
		},

	}
};
Passage p_driving_over_the_deep = {
	2, {
		{
			4, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to b", "ehold", "Came ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_drown,
			{"driving over the deep", }
		},

	}
};
Passage p_drown = {
	3, {
		{
			5, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to b", "ehold", "Came driving over the deep", "to ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_realms,
			{"drown", }
		},
		{
			3, false, {true,false,false,false,false,false,false,false,false,false,}, &p_realms,
			{" his people", "", "", }
		},

	}
};
Passage p_realms = {
	3, {
		{
			8, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to b", "ehold", "Came driving over the deep", "to dr", "own his people,", "Ranging directly from ", "the ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_spite,
			{"realms", }
		},
		{
			4, false, {true,false,false,false,false,false,false,false,false,false,}, &p_spite,
			{"", "of the west", "", "", }
		},

	}
};
Passage p_spite = {
	2, {
		{
			11, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to b", "ehold", "Came driving over the deep", "to dr", "own his people,", "Ranging directly from ", "the realms", "of the west", "And soaring in ", "", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_surging_sea,
			{"spite", }
		},

	}
};
Passage p_surging_sea = {
	1, {
		{
			13, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to b", "ehold", "Came driving over the deep", "to dr", "own his people,", "Ranging directly from ", "the realms", "of the west", "And soaring in ", "spite", "over the surging sea.", "", }
		},

	}
};
