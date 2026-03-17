import xml.etree.ElementTree as ET
from flask import Flask, request, Response

app = Flask(__name__)

# Basic dictionary to track current queue per client IP
client_queues = {}

def generate_story_nsml(title="MOCK STORY", video_id=None, page_number=None):
        
    return f"""<nsml:nsml xmlns:nsml="http://avid.com/nsml" version="3.1">
	<head>
		<formname>VTV-SF</formname>
		<storyid>1b62c310:0198e185:698923ca</storyid>
	</head>
	<fields>
		<string id="title">{title}</string>
		<string id="video-id">{video_id}</string>
        <string id="page-number">{page_number}</string>
	</fields>
	<body>
		<p><cc>##CG:</cc></p>
		<p><cc>MOCK CONTENT</cc></p>
	</body>
</nsml:nsml>"""

def create_soap_response(inner_xml):
    return f"""<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" 
               xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
               xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    {inner_xml}
  </soap:Body>
</soap:Envelope>"""

@app.route('/inewswebservice/services/inewssystem', methods=['POST'])
def inewssystem():
    data = request.data.decode('utf-8')
    print("--- INEWSSystem Request ---")
    print(data)
    
    if "Connect" in data and "Disconnect" not in data and "IsConnected" not in data:
        resp = '<ConnectResponse xmlns="http://avid.com/inewssystem/types"></ConnectResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')
        
    if "IsConnected" in data:
        resp = '<IsConnectedResponse xmlns="http://avid.com/inewssystem/types"><IsConnected>true</IsConnected></IsConnectedResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')
        
    if "Disconnect" in data:
        resp = '<DisconnectResponse xmlns="http://avid.com/inewssystem/types"></DisconnectResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')

    if "GetFolderChildren" in data:
        import re
        match = re.search(r'<([^:]+:)?FolderFullName[^>]*>(.*?)</([^:]+:)?FolderFullName>', data)
        parent_folder = match.group(2) if match else "VO_BAN_TIN"
        # Mocking 2 child queues
        resp = f"""<GetFolderChildrenResponse xmlns="http://avid.com/inewssystem/types">
            <ParentFolderFullName>{parent_folder}</ParentFolderFullName>
            <Children>
                <Type>Queue</Type>
                <Name>BAN_TIN_9H00</Name>
                <FullName>{parent_folder}.BAN_TIN_9H00</FullName>
            </Children>
            <Children>
                <Type>Queue</Type>
                <Name>BAN_TIN_10H00</Name>
                <FullName>{parent_folder}.BAN_TIN_10H00</FullName>
            </Children>
            <Children>
                <Type>Queue</Type>
                <Name>BAN_TIN_12H00</Name>
                <FullName>{parent_folder}.BAN_TIN_12H00</FullName>
            </Children>
        </GetFolderChildrenResponse>"""
        return Response(create_soap_response(resp), mimetype='text/xml')

    # Default fallback
    return Response(create_soap_response('<UnknownResponse />'), mimetype='text/xml')

@app.route('/inewswebservice/services/inewsqueue', methods=['POST'])
def inewsqueue():
    data = request.data.decode('utf-8')
    print("--- INEWSQueue Request ---")
    print(data)
    
    if "SetCurrentQueue" in data:
        import re
        match = re.search(r'<([^:]+:)?QueueFullName[^>]*>(.*?)</([^:]+:)?QueueFullName>', data)
        if match:
            client_queues[request.remote_addr] = match.group(2)
            
        resp = '<SetCurrentQueueResponse xmlns="http://avid.com/inewsqueue/types"></SetCurrentQueueResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')
        
    if "GetStories" in data:
        current_queue = client_queues.get(request.remote_addr, "VTV4.04_VO_BAN_TIN.BAN_TIN_9H00")
        
        # Determine stories based on queue name
        if "10H00" in current_queue:
            stories_data = [
                ("CHÀO BUỔI SÁNG 10H - THỜI TIẾT", "TS100001WTH", "101"),
                ("TIN QUỐC TẾ - BẦU CỬ MỸ", "TS100002INT", "102"),
                ("TIN TRONG NƯỚC - GIÁ VÀNG TĂNG", "TS100003DOM", "103"),
                ("GÓC CHUYÊN GIA - BẤT ĐỘNG SẢN", "TS100004EXP", "104")
            ]
        elif "12H00" in current_queue:
            stories_data = [
                ("BẢN TIN TRƯA - TÓM TẮT SỰ KIỆN", "TS120001SUM", "201"),
                ("TIN GIAO THÔNG - TẮC ĐƯỜNG TRẦN DUY HƯNG", "TS120002TRA", "202"),
                ("NHỊP ĐẬP THỂ THAO - NGOẠI HẠNG ANH", "TS120003SPO", "203"),
                ("PHÓNG SỰ - MÙA LŨ MIỀN TÂY", "TS120004REP", "204"),
                ("QUỐC TẾ - HỘI NGHỊ THƯỢNG ĐỈNH", "TS120005INT", "205")
            ]
        else: # Default 9H00
            stories_data = [
                ("TIN MỚI - KINH TẾ", "TS020926BKC", "26"),
                ("GÓC NHÌN ĐỘC GIẢ - THỂ THAO", "TS123456CUA", "42"),
                ("ĐIỂM BÁO SÁNG - TÌNH HÌNH CHÂU ÂU", "TS000003EUR", "03"),
                ("PHIM TÀI LIỆU - NGHỆ THUẬT ĐƯỜNG PHỐ", "TS000004DOC", "04")
            ]
            
        stories_xml = ""
        for title, video_id, page_number in stories_data:
            story_nsml = generate_story_nsml(title=title, video_id=video_id, page_number=page_number)
            locator = f"loc_{video_id}"
            stories_xml += f'''
            <Stories>
                <story:FullPath>{current_queue}</story:FullPath>
                <story:QueueLocator>{locator}</story:QueueLocator>
                <story:StoryAsNSML><![CDATA[{story_nsml}]]></story:StoryAsNSML>
            </Stories>'''
        
        resp = f"""<GetStoriesResponse xmlns="http://avid.com/inewsqueue/types" xmlns:story="http://avid.com/inewsstory/types">
            {stories_xml}
        </GetStoriesResponse>"""
        return Response(create_soap_response(resp), mimetype='text/xml')

    # Default fallback
    return Response(create_soap_response('<UnknownResponse />'), mimetype='text/xml')

if __name__ == '__main__':
    print("Starting iNews Mock Server on port 8080...")
    app.run(host='0.0.0.0', port=8080)
