#include "Passages.h"

Passage p_StoryTitle = {
	1, {
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"Arthur's Dream of the Dragon and the Bear", }
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
			2, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{", on a huge vessel", "with a host of knights", }
		},

	}
};
Passage p_completely_equipped_cabin = {
	2, {
		{
			3, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King Arthur, on a huge vessel", "with a host of knights,", "Was enclosed in a ", }
		},
		{
			2, false, {true,false,false,false,false,false,false,false,false,false,}, &p_richly,
			{"completely equipped", "cabin", }
		},

	}
};
Passage p_richly = {
	5, {
		{
			5, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King Arthur, on a huge vessel", "with a host of knights,", "Was enclosed in a completely equipped", "cabin,", "Resting on a ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_sent_him_to_sleep,
			{"richly", }
		},
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{" arrayed ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_sent_him_to_sleep,
			{"bed", }
		},
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{".", }
		},

	}
};
Passage p_sent_him_to_sleep = {
	3, {
		{
			7, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"King Arthur, on a huge vessel", "with a host of knights,", "Was enclosed in a completely equipped", "cabin,", "Resting on a richly arrayed bed.", "And the swaying on the sea", "", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_dreamed,
			{"sent him to sleep", }
		},
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{".", }
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
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{" to behold", }
		},

	}
};
Passage p_driving_over_the_deep = {
	2, {
		{
			3, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to behold", "Came ", }
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
			4, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to behold", "Came driving over the deep", "to ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_realms,
			{"drown", }
		},
		{
			1, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{" his people", }
		},

	}
};
Passage p_realms = {
	3, {
		{
			5, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to behold", "Came driving over the deep", "to drown his people,", "Ranging directly from the ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_spite,
			{"realms", }
		},
		{
			2, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"", "of the west", }
		},

	}
};
Passage p_spite = {
	2, {
		{
			7, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to behold", "Came driving over the deep", "to drown his people,", "Ranging directly from the realms", "of the west", "And soaring in ", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_surging_sea,
			{"spite", }
		},

	}
};
Passage p_surging_sea = {
	2, {
		{
			8, false, {false,false,false,false,false,false,false,false,false,false,}, NULL,
			{"He dreamed that a dragon", "dreadful to behold", "Came driving over the deep", "to drown his people,", "Ranging directly from the realms", "of the west", "And soaring in spite", "over the surging sea.", }
		},
		{
			1, false, {true,false,false,false,false,false,false,false,false,false,}, &p_Start,
			{" ", }
		},

	}
};
