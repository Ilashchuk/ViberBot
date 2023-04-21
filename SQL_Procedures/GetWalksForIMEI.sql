
CREATE PROCEDURE [dbo].[GetWalksForIMEI]
	@imei varchar(50)
AS
BEGIN
	SELECT
        IMEI,
        MIN(date_track) AS walk_start,
        MAX(date_track) AS walk_end,
        dbo.distance_between_points(MIN(latitude), MIN(longitude), MAX(latitude), MAX(longitude)) AS walk_distance,
        DATEDIFF(MINUTE, MIN(date_track), MAX(date_track)) AS walk_duration
    FROM
        TrackLocation
	WHERE IMEI = @imei
    GROUP BY
        imei,
        DATEDIFF(MINUTE, 0, date_track) / 30
    HAVING
        COUNT(*) > 1
	ORDER BY walk_start DESC;
END