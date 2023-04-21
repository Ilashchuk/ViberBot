
CREATE PROCEDURE [dbo].[GetTotalWalkForIMEI]
	@imei varchar(50)
AS
BEGIN
    DECLARE @walks TABLE (
        Id INT PRIMARY KEY IDENTITY(1,1),
        imei VARCHAR(50),
        walk_distance FLOAT,
        walk_duration FLOAT,
        walk_count INT
    );

    INSERT INTO @walks (imei, walk_distance, walk_duration, walk_count)
    SELECT
        IMEI,
        SUM(dbo.distance_between_points(latitude1, longitude1, latitude2, longitude2)) AS walk_distance,
        SUM(DATEDIFF(MINUTE, date1, date2)) AS walk_duration,
        COUNT(*) AS walk_count
    FROM (
        SELECT
            IMEI,
            latitude1,
            longitude1,
            date1,
            latitude2,
            longitude2,
            date2
        FROM (
            SELECT
                IMEI,
                latitude1,
                longitude1,
                date1,
                latitude2,
                longitude2,
                date2,
                ROW_NUMBER() OVER (PARTITION BY IMEI, walk_id ORDER BY date1) AS rn1,
                ROW_NUMBER() OVER (PARTITION BY IMEI, walk_id ORDER BY date2) AS rn2
            FROM (
                SELECT
                    IMEI,
                    latitude1,
                    longitude1,
                    MIN(date_track) AS date1,
                    latitude2,
                    longitude2,
                    MAX(date_track) AS date2,
                    DATEDIFF(MINUTE, 0, date_track) / 30 AS walk_id
                FROM (
                    SELECT
                        IMEI,
                        latitude AS latitude1,
                        longitude AS longitude1,
                        date_track,
                        LEAD(latitude) OVER (PARTITION BY IMEI ORDER BY date_track) AS latitude2,
                        LEAD(longitude) OVER (PARTITION BY IMEI ORDER BY date_track) AS longitude2
                    FROM TrackLocation
                    WHERE IMEI = @imei
                ) t
                WHERE latitude2 IS NOT NULL AND longitude2 IS NOT NULL
                GROUP BY
                    IMEI,
                    latitude1,
                    longitude1,
                    latitude2,
                    longitude2,
                    DATEDIFF(MINUTE, 0, date_track) / 30
            ) t
            --WHERE rn1 = 1 OR rn2 = 1
        ) t
        WHERE rn1 = 1 AND rn2 = 1
    ) t
    GROUP BY
        IMEI;

    SELECT 
		Id,
        walk_count,
        walk_distance,
        walk_duration
    FROM
        @walks
    WHERE
        imei = @imei
    ORDER BY
        walk_count DESC;
END