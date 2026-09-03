CREATE OR REPLACE FUNCTION Cut_Tracker_Number_Of_Lifts_Get()
RETURNS TABLE (
    thickness double precision,
    width double precision,
	length double precision,
    numberOfLifts integer
)
LANGUAGE sql
AS $$
SELECT thickness
	  ,width
	  ,length
	  ,COUNT(*) 
FROM "Bundles"
WHERE "timeProcessed" > CURRENT_DATE::timestamp
GROUP BY thickness, width, length
ORDER BY length, thickness, width
$$;