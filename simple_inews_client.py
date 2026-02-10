import socket
import xml.etree.ElementTree as ET
import re

# Cấu hình kết nối
HOST = '127.0.0.1'  # Updated to match App.config ServerIP
PORT = 3000
BUFFER_SIZE = 4096 * 10  # Tăng buffer nếu dữ liệu lớn

def send_command(command):
    """Gửi lệnh TCP và nhận phản hồi XML"""
    print(f"Connecting to {HOST}:{PORT}...")
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((HOST, PORT))
            print(f"Sending command: {command}")
            s.sendall(command.encode('utf-8'))
            
            # Nhận dữ liệu (có thể cần loop để nhận hết nếu lớn)
            data = b""
            while True:
                part = s.recv(BUFFER_SIZE)
                if not part:
                    break
                data += part
            
            response = data.decode('utf-8')
            return response
    except Exception as e:
        print(f"Error: {e}")
        return None

def parse_xml_response(xml_data):
    """Parse XML trả về giống logic của FormInews"""
    try:
        if not xml_data or "<data>" not in xml_data:
            print("Invalid or empty XML response")
            return

        # Wrap trong thẻ root giả nếu cần thiết, hoặc parse trực tiếp
        # Server trả về dạng <data><Stories>...</Stories></data>
        root = ET.fromstring(xml_data)
        
        stories = root.findall(".//Stories")
        print(f"Found {len(stories)} stories.")
        
        for idx, story in enumerate(stories):
            # Lấy Content giống logic FormInews
            content_elem = story.find("Content")
            story_name = story.find("Name") # Giả sử có trường Name
            
            name = story_name.text if story_name is not None else f"Story {idx+1}"
            content = content_elem.text if content_elem is not None else ""
            
            print(f"\n--- {name} ---")
            
            # Mô phỏng logic Extract Video ID từ FormInews
            # (Regex ví dụ: tìm chuỗi số 7-14 ký tự)
            video_id_match = re.search(r'\b\d{7,14}\b', content)
            if video_id_match:
                print(f"Video ID: {video_id_match.group(0)}")
            else:
                print("No Video ID found")
                
            # Mô phỏng logic Extract CG (FormInews.ExtractCGBlocks)
            if "##CG:" in content:
                print("Has CG Data")
                
    except ET.ParseError as e:
        print(f"XML Parse Error: {e}")

if __name__ == "__main__":
    # 1. Lấy cây thư mục (nếu cần xem cấu trúc)
    # response_tree = send_command("GET_TREE")
    # print("Tree Data:", response_tree)
    
    # 2. Lấy danh sách tin từ Queue (Ví dụ: Running.Runown)
    # Thay 'Running.Runown' bằng tên Queue thực tế trong iNews
    queue_name = "Running.Runown" 
    xml_response = send_command(f"GET_STORIES|{queue_name}")
    
    if xml_response:
        # print("Raw XML:", xml_response[:500] + "...") # In 500 ký tự đầu
        parse_xml_response(xml_response)
