struct map{

};
struct map * map_new(float w, float h);
void map_delete(struct map *m);
int map_insert(struct map *m, const char * mode);
void map_erase(struct map *m, int handle);
void map_location(struct map *m, int handle, float pos[3]);
void map_moveto(struct map *m, int handle, float target[3]);
void map_follow(struct map *m, int handle, int who);
void map_speed(struct map *m, int handle, float speed);
void map_move(struct map *m, float tick);
const float * map_velocity(struct map *m, int handle);
const float * map_position(struct map *m, int handle);

int map_around(struct map *m, int handle, float rad_short, float rad_long, 
    void (*around_cb)(void *ud, int handle, int enter), void *ud);