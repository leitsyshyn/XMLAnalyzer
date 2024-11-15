<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" indent="yes" encoding="UTF-8" />

	<xsl:template match="/">
		<html>
			<head>
				<title>Filtered Staff Results</title>
				<style>
					table {
					width: 100%;
					border-collapse: collapse;
					}
					th, td {
					border: 1px solid #ddd;
					padding: 8px;
					text-align: left;
					}
					th {
					background-color: #f2f2f2;
					}
				</style>
			</head>
			<body>
				<h1>Filtered Staff Results</h1>
				<table>
					<tr>
						<th>First Name</th>
						<th>Middle Name</th>
						<th>Last Name</th>
						<th>Faculty</th>
						<th>Department</th>
						<th>Degree Level</th>
						<th>Degree Specialization</th>
						<th>Degree Award Date</th>
						<th>Title</th>
						<th>Title Start Date</th>
						<th>Title End Date</th>
					</tr>
					<xsl:for-each select="//Scientist">
						<tr>
							<td>
								<xsl:value-of select="FirstName" />
							</td>
							<td>
								<xsl:value-of select="MiddleName" />
							</td>
							<td>
								<xsl:value-of select="LastName" />
							</td>
							<td>
								<xsl:value-of select="Faculty" />
							</td>
							<td>
								<xsl:value-of select="Department" />
							</td>
							<td>
								<xsl:value-of select="DegreeLevel" />
							</td>
							<td>
								<xsl:value-of select="DegreeSpecialization" />
							</td>
							<td>
								<xsl:value-of select="DegreeAwardDate" />
							</td>
							<td>
								<xsl:value-of select="TitleName" />
							</td>
							<td>
								<xsl:value-of select="TitleStartDate" />
							</td>
							<td>
								<xsl:value-of select="TitleEndDate" />
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
