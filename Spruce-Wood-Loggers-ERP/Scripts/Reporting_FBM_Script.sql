CREATE OR REPLACE FUNCTION Cut_Tracker_Daily_Total_FBM_Get()
RETURNS double precision
LANGUAGE sql
AS $$
SELECT SUM((length * thickness * width * "numPieces")/12) 
FROM "Bundles"
WHERE "timeProcessed" > CURRENT_DATE::timestamp
$$;