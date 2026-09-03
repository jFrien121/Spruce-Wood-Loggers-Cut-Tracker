CREATE OR REPLACE FUNCTION public.cut_tracker_standard_num_pieces_get(
	thicknessParam double precision, widthParam double precision)
    RETURNS TABLE(standardNumPieces integer) 
    LANGUAGE 'sql'

AS $$
SELECT "numPieces"
FROM "StandardNumPieces" AS s
WHERE EXISTS ( SELECT id FROM "StandardSizeRelationships" AS sr
				WHERE sr."StandardNumPiecesId" = s.id
					AND EXISTS (SELECT id 
									FROM "CutSizes" AS c
									WHERE c.thickness = thicknessParam 
										AND c.width = widthParam
										AND c.id = sr."CutSizeId"))
$$;

