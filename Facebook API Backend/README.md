cruk
====

CRUK Hack 2013

API
===
"Dummy" version with a placeholder Unity player - handles login (sort of)
https://apps.facebook.com/cruk_hack/

GET /token (gets current user's access token - doesn't work for mobile)

GET /users/current?access_token=[TOKEN] - gets current user (owner of access token)

GET /users/picture?id=[USER_FB_ID] - gets user profile picture, no access token required

GET /users/friends/?access_token=[TOKEN] - gets user's list of friends

GET /users/friends/?playing=1&access_token=[TOKEN] - gets list of playing friends

GET /achievements/ - gets all achievements available in the system

POST /achievements/?user_id=[USER_FB_ID]&achievement_id=[ID] - posts the achievement on the user's wall

POST /achievements/register?achievement_id=[ID] - register an achievement. All achievements added to the database need to be registered before being able to use them
